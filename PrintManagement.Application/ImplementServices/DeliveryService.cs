using Microsoft.AspNetCore.Http;
using PrintManagement.Application.Handle.HandleEmail;
using PrintManagement.Application.InterfaceServices;
using PrintManagement.Application.Payloads.Mappers;
using PrintManagement.Application.Payloads.RequestModels.DeliveryRequests;
using PrintManagement.Application.Payloads.RequestModels.InputRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataDelivery;
using PrintManagement.Application.Payloads.Responses;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using PrintManagement.Domain.InterfaceRepositories.InterfaceUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.ImplementServices
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IBaseReposiroty<Delivery> _baseDeliveryRepository;
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly IBaseReposiroty<ConfirmReceiptOfGoodsFromCustomer> _confirmReceiptOfGoodsFromCustomerRepository;
        private readonly DeliveryConverter _deliveryConverter;
        private readonly IUserRepository<User> _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBaseReposiroty<Team> _teamRepository;
        private readonly IBaseReposiroty<ShippingMethod> _shippingMethodRepository;
        private readonly IBaseReposiroty<Project> _projectRepository;
        private readonly IBaseReposiroty<Customer> _customerRepository;
        private readonly IBaseReposiroty<Notification> _notificationRepository;
        private readonly IBaseReposiroty<Bill> _billRepository;

        private readonly IEmailService _emailService;
        public DeliveryService(IBaseReposiroty<Delivery> baseDeliveryRepository, IBaseReposiroty<ConfirmReceiptOfGoodsFromCustomer> confirmReceiptOfGoodsFromCustomerRepository, DeliveryConverter deliveryConverter, IHttpContextAccessor contextAccessor, IUserRepository<User> userRepository, IBaseReposiroty<User> baseUserRepository, IBaseReposiroty<Team> teamRepository, IBaseReposiroty<Customer> customerRepository, IBaseReposiroty<Notification> notificationRepository, IEmailService emailService, IBaseReposiroty<Project> projectRepository, IBaseReposiroty<ShippingMethod> shippingMethodRepository, IBaseReposiroty<Bill> billRepository)
        {
            _baseDeliveryRepository = baseDeliveryRepository;
            _confirmReceiptOfGoodsFromCustomerRepository = confirmReceiptOfGoodsFromCustomerRepository;
            _deliveryConverter = deliveryConverter;
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
            _baseUserRepository = baseUserRepository;
            _teamRepository = teamRepository;
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _projectRepository = projectRepository;
            _customerRepository = customerRepository;
            _shippingMethodRepository = shippingMethodRepository;
            _billRepository = billRepository;
        }

        public async Task<ResponseObject<DataResponseDelivery>> CreateDelivery(Request_CreateDelivery request)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "Người dùng chưa được xác thực",
                        Data = null
                    };
                }
                var user = await _baseUserRepository.GetAsync(x => x.Id == Guid.Parse(currentUser.FindFirst("Id").Value));
                var team = await _teamRepository.GetAsync(x => x.Id == user.TeamId);
                if (!currentUser.IsInRole("Manager") && !team.Name.Equals("Delivery"))
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Bạn không có quyền thực hiện chức năng này",
                        Data = null
                    };
                }

                var project = await _projectRepository.GetByIDAsync(request.ProjectId);
                if (project == null)
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Dự án không tồn tại",
                        Data = null
                    };
                }
                if (!project.ProjectStatus.ToString().Equals("Completed"))
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Dự án chưa được hoàn thành! Không thể giao đến cho khách hàng",
                        Data = null
                    };
                }

                var shippingMethod = await _shippingMethodRepository.GetAsync(x => x.Id == request.ShippingMethodId);
                if(shippingMethod == null)
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Phương thức giao hàng không tồn tại",
                        Data = null
                    };
                }

                var deliver = await _baseUserRepository.GetByIDAsync(request.DeliverId);
                if(deliver == null)
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Thông tin người giao hàng không tồn tại",
                        Data = null
                    };
                }
                var teamDeliver = await _teamRepository.GetAsync(x => x.Id == deliver.TeamId);
                var checkRole = _userRepository.GetRolesOfUserAsync(deliver).Result.Contains("Deliver");
                if (!checkRole || !teamDeliver.Name.Equals("Delivery"))
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Data = null,
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Người dùng không có quyền hoặc không nằm trong phòng ban Delivery"
                    };
                }

                var customer = await _customerRepository.GetAsync(x => x.Id == request.CustomerId);
                if(customer == null)
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Thông tin khách hàng không tồn tại",
                        Data = null
                    };
                }
                Delivery delivery = new Delivery
                {
                    DeliveryAddress = customer.Address,
                    IsActive = true,
                    CustomerId = request.CustomerId,
                    DeliverId = request.DeliverId,
                    DeliveryStatus = Domain.Enumerates.DeliveryStatusEnum.Waiting,
                    EstimateDeliveryTime = request.EstimateDeliveryTime,
                    Id = Guid.NewGuid(),
                    ProjectId = request.ProjectId,
                    ShippingMethodId = request.ShippingMethodId,
                };
                delivery = await _baseDeliveryRepository.CreateAsync(delivery);
                Notification notification = new Notification
                {
                    IsActive = true,
                    Content = $"Bạn đã được chỉ định giao đơn hàng {project.ProjectName}! Thời gian bắt đầu giao hàng là vào thời điểm này",
                    Id = Guid.NewGuid(),
                    IsSeen = false,
                    Link = "",
                    UserId = deliver.Id
                };
                notification = await _notificationRepository.CreateAsync(notification);
                ConfirmReceiptOfGoodsFromCustomer confirm = new ConfirmReceiptOfGoodsFromCustomer
                {
                    IsActive = true,
                    ConfirmReceiptOfGoods = Domain.Enumerates.ConfirmReceiptOfGoodsEnum.NotReceived,
                    DeliveryId = delivery.Id,
                    Id = Guid.NewGuid()
                };
                confirm = await _confirmReceiptOfGoodsFromCustomerRepository.CreateAsync(confirm);

                return new ResponseObject<DataResponseDelivery>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Tạo thông tin giao hàng thành công",
                    Data = _deliveryConverter.EntityToDTO(delivery),
                };

            }catch (Exception ex)
            {
                return new ResponseObject<DataResponseDelivery>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ResponseObject<DataResponseDelivery>> CustomerConfirmDelivery(Guid deliveryId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseObject<DataResponseDelivery>> ShipperConfirmDelivery(Guid shipperId, Request_ShipperConfirmDelivery request)
        {
            var deliver = await _baseUserRepository.GetByIDAsync(shipperId);
            try
            {
                var team = await _teamRepository.GetAsync(x => x.Id == deliver.TeamId);
                if (team == null)
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Phòng ban không tồn tại",
                        Data = null
                    };
                }
                if (!_userRepository.GetRolesOfUserAsync(deliver).Result.Contains("Deliver") || !team.Name.Equals("Delivery"))
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Bạn không có quyền thực hiện chức năng này",
                        Data = null
                    };
                }
                var delivery = await _baseDeliveryRepository.GetByIDAsync(request.DeliveryId);
                if (delivery == null)
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Thông tin giao hàng không tồn tại",
                        Data = null
                    };
                }
                if(shipperId != delivery.DeliverId)
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Bạn không phải là người giao đơn hàng này",
                        Data = null
                    };
                }
                if (delivery.DeliveryStatus.ToString().Equals("Delivered"))
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Đơn hàng đã được giao từ trước đó",
                        Data = null
                    };
                }
                delivery.DeliveryStatus = Domain.Enumerates.DeliveryStatusEnum.Delivering;
                deliver.UpdateTime = DateTime.Now;
                await _baseDeliveryRepository.UpdateAsync(delivery);

                var project = await _projectRepository.GetAsync(x => x.Id == delivery.ProjectId);
                

                var customer = await _customerRepository.GetByIDAsync(project.CustomerId);
                var confirm = await _confirmReceiptOfGoodsFromCustomerRepository.GetAsync(x => x.DeliveryId == delivery.Id);
                var message = new EmailMessage(new string[] { customer.Email }, "Thông báo đơn hàng của bạn: ", $"Nếu bạn đã nhận được hàng, vui lòng xác nhận với chúng tôi và phản hồi về chất lượng đơn hàng: {confirm.ConfirmReceiptOfGoods = request.ConfirmStatus}");
                var responseMessage = _emailService.SendEmail(message);

                await _confirmReceiptOfGoodsFromCustomerRepository.UpdateAsync(confirm);

                var bill = await _billRepository.GetAsync(x => x.ProjectId == project.Id && x.BillStatus == Domain.Enumerates.BillStatusEnum.UnPaid);
                if(bill != null)
                {
                    bill.BillStatus = Domain.Enumerates.BillStatusEnum.Paid;
                    await _billRepository.UpdateAsync(bill);
                }
                delivery.DeliveryStatus = Domain.Enumerates.DeliveryStatusEnum.Delivered;
                await _baseDeliveryRepository.UpdateAsync(delivery);
                project.ProjectStatus = Domain.Enumerates.ProjectStatusEnum.Delivered;
                await _projectRepository.UpdateAsync(project);
                return new ResponseObject<DataResponseDelivery>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Đơn hàng đã được giao thành công! khách hàng cũng đã nhận hàng",
                    Data = _deliveryConverter.EntityToDTO(delivery)
                };

            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseDelivery>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ResponseObject<DataResponseDelivery>> ShipperConfirmOrderDelivery(Guid shipperId, Request_ShipperConfirmOrderDelivery request)
        {
            var deliver = await _baseUserRepository.GetByIDAsync(shipperId);
            try
            {
                var team = await _teamRepository.GetAsync(x => x.Id == deliver.TeamId);
                if(team == null)
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Phòng ban không tồn tại",
                        Data = null
                    };
                }
                if (!_userRepository.GetRolesOfUserAsync(deliver).Result.Contains("Deliver") || !team.Name.Equals("Delivery"))
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Bạn không có quyền thực hiện chức năng này",
                        Data = null
                    };
                }
                var delivery = await _baseDeliveryRepository.GetByIDAsync(request.DeliveryId);
                if(delivery == null)
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Thông tin giao hàng không tồn tại",
                        Data = null
                    };
                }
                if(shipperId != delivery.DeliverId)
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Data = null,
                        Message = "Bạn không phải là người giao đơn hàng này"
                    };
                }
                if (delivery.DeliveryStatus.ToString().Equals("Delivered"))
                {
                    return new ResponseObject<DataResponseDelivery>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Đơn hàng đã được giao từ trước đó",
                        Data = null 
                    };
                }
                delivery.DeliveryStatus = Domain.Enumerates.DeliveryStatusEnum.Delivering;
                deliver.UpdateTime = DateTime.Now;
                await _baseDeliveryRepository.UpdateAsync(delivery);

                var project = await _projectRepository.GetAsync(x => x.Id == delivery.ProjectId);
                Notification notification = new Notification
                {
                    IsActive = true,
                    Content = "Người giao hàng đã nhận giao đơn!",
                    Id = Guid.NewGuid(),
                    IsSeen = false,
                    Link = "",
                    UserId = project.LeaderId
                };

                notification = await _notificationRepository.CreateAsync(notification);

                var customer = await _customerRepository.GetByIDAsync(project.CustomerId);
                var message = new EmailMessage(new string[] { customer.Email }, "Thông báo đơn hàng của bạn: ", "Đơn đặt hàng của bạn đang được giao");
                var responseMessage = _emailService.SendEmail(message);

                return new ResponseObject<DataResponseDelivery>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Xác nhận nhiệm vụ giao đơn hàng",
                    Data = _deliveryConverter.EntityToDTO(delivery)
                };

            }catch (Exception ex)
            {
                return new ResponseObject<DataResponseDelivery>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
        public async Task<IQueryable<DataResponseDelivery>> GetAllDelivery(Request_InputDelivery input)
        {
            var query = await _baseDeliveryRepository.GetAllAsync(record => record.IsActive == true);
            if (input.ProjectId.HasValue)
            {
                query = query.Where(record => record.ProjectId == input.ProjectId);
            }
            if(input.CustomerId.HasValue)
            {
                query = query.Where(record => record.CustomerId == input.CustomerId);
            }
            if (input.DeliverId.HasValue)
            {
                query = query.Where(record => record.DeliverId == input.DeliverId);
            }
            if (input.DeliveryStatus.HasValue)
            {
                query = query.Where(record => record.DeliveryStatus == input.DeliveryStatus);
            }
            return query.Select(item => _deliveryConverter.EntityToDTO(item));
        }

        public async Task<DataResponseDelivery> GetDeliveryById(Guid id)
        {
            var query = await _baseDeliveryRepository.GetByIDAsync(id);
            return _deliveryConverter.EntityToDTO(query);
        }
    }
}
