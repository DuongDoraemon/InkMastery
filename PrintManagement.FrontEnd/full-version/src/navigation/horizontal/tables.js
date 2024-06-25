export default [
  {
    title: "Tables",
    icon: { icon: "tabler-layout-grid" },
    children: [
      {
        title: "Simple Table",
        icon: { icon: "tabler-table" },
        to: "tables-simple-table",
      },
      {
        title: "Data Table",
        icon: { icon: "tabler-layout-grid" },
        to: "tables-data-table",
        meta: {
          auth: ["Admin"],
        },
      },
      {
        title: "Data Table",
        icon: { icon: "tabler-layout-grid" },
        to: "pages-tables-warehouse",
      },
    ],
  },
];
