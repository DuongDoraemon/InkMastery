import { roleEnum } from "@/helper/roleEnum";
const userInfo = JSON.parse(localStorage.getItem("userInfo"));
export default [
  {
    title: "Quản lý nhân viên",
    icon: { icon: "tabler-users-group" },
    roleId: [roleEnum.Admin],
    to: "tables-simple-table",
  },
  {
    title: "Quản lý khách hàng",
    icon: { icon: "tabler-user-shield" },
    roleId: [roleEnum.Admin, roleEnum.Employee],
    to: "tables-data-table",
  },
].filter((menuItem) => {
  const userPermissions = Array.isArray(userInfo.Permission)
    ? userInfo.Permission
    : [userInfo.Permission];
  const userRoleIds = userPermissions.map((perm) => roleEnum[perm]);
  return menuItem.roleId
    ? menuItem.roleId.some((roleId) => userRoleIds.includes(roleId))
    : true;
});
