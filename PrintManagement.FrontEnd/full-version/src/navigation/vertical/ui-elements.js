import { roleEnum } from "@/helper/roleEnum";
const userInfo = JSON.parse(localStorage.getItem("userInfo"));

export default [
  {
    title: "Trang chủ",
    roleId: [
      roleEnum.Admin,
      roleEnum.Deliver,
      roleEnum.Designer,
      roleEnum.Leader,
      roleEnum.Employee,
      roleEnum.Manager,
    ],
    icon: { icon: "tabler-home" },
    to: "pages-cards-card-basic",
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
