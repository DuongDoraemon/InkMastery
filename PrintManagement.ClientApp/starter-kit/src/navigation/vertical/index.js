
const userInfo = JSON.parse(localStorage.getItem('userInfo'))

export default [
  {
    title: 'Trang chủ',
    to: { name: 'index' },
    icon: { icon: 'mdi-home' },
  },
  {
    title: 'Quản lý bất động sản',
    icon: { icon: 'mdi-home-city' },
    children: [
      {
        title: 'Danh sách BDS', 
        to: 'product-product-management',
      },

      // { title: 'Preview', to: { name: 'apps-invoice-preview-id', params: { id: '5036' } } },
      // { title: 'Edit', to: { name: 'apps-invoice-edit-id', params: { id: '5036' } } },
      // { title: 'Add', to: 'apps-invoice-add' },
    ],
  },

  {
    title: 'Phiếu xem nhà',
    icon: { icon: 'mdi-file-document-edit' },
    children: [
      { 
        title: 'Danh sách phiếu', 
        to: 'houseViewing-bill',
      },
    ],
  },
  {
    title: 'Quản lý đội nhóm',
    icon: { icon: 'mdi-account-group' },
    children: [
      { 
        title: 'Danh sách đội nhóm', 
        to: 'team-team-list',
      },
    ],
  },
  {
    title: 'Quản lý tài khoản',
    icon: { icon: 'mdi-account' },
    children: [
      { 
        title: 'Danh sách người dùng', 
        to: 'user-list-users',
      },
    ],
  },
  {
    title: 'Thống kê',
    icon: { icon: 'mdi-chart-bar' },
    children: [
      { 
        title: 'Doanh số công ty', 
        to: 'statistic-DoanhSoCongTy-company-chart',
      },
      { 
        title: 'Doanh số phòng ban', 
        to: 'statistic-DoanhSoPhongBanCuaToi-team-chart',
      },
    ],
  },
].filter(x=> (x.roleId && x.roleId.findIndex(y => y == userInfo.RoleId) !== -1) || !x.roleId)
