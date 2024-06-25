import { roleEnum } from "@/helper/roleEnum";
const userInfo = JSON.parse(localStorage.getItem("userInfo"));

export default [
  {
    icon: { icon: "tabler-chart-infographic" },
    title: "Thống kê",
    roleId: [roleEnum.Admin],
    to: "dashboards-analytics",
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
