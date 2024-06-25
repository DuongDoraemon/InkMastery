import { roleEnum } from "@/helper/roleEnum";
const userInfo = JSON.parse(localStorage.getItem("userInfo"));

export default [
  {
    title: "Quản lý kho",
    icon: { icon: "tabler-packages" },
    roleId: [
      roleEnum.Admin,
      roleEnum.Designer,
      roleEnum.Employee,
      roleEnum.Leader,
    ],
    to: "apps-invoice-list",
  },

  {
    icon: { icon: "tabler-tir" },
    title: "Quản lý giao hàng",
    roleId: [roleEnum.Admin, roleEnum.Deliver],
    to: "apps-roles",
  },
  {
    title: "Quản lý dự án",
    icon: { icon: "tabler-photo" },
    roleId: [
      roleEnum.Admin,
      roleEnum.Designer,
      roleEnum.Employee,
      roleEnum.Leader,
    ],
    to: "pages-dialog-examples",
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
