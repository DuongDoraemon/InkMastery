export default [
  {
    title: 'CRM',
    to: 'dashboards-crm',
    icon: { icon: 'tabler-3d-cube-sphere' },
  },
  {
    title: 'Dashboards',
    icon: { icon: 'tabler-smart-home' },
    children: [
      {
        title: 'Analytics',
        to: 'dashboards-analytics',
        icon: { icon: 'tabler-chart-pie-2' },
      },
      {
        title: 'eCommerce',
        to: 'dashboards-ecommerce',
        icon: { icon: 'tabler-atom-2' },
      },
      
    ],
  },
]
