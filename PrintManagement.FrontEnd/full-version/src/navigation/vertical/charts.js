import { roleEnum } from "@/helper/roleEnum";
const userInfo = JSON.parse(localStorage.getItem("userInfo"));
export default [
  {
    title: "Quản lý phòng ban",
    roleId: [roleEnum.Admin, roleEnum.Manager],
    icon: { icon: "tabler-brand-teams" },
    to: "charts-apex-chart",
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
