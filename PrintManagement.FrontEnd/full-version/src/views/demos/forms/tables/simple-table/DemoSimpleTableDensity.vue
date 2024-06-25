<template>
  <VTable density="compact" class="text-no-wrap">
    <thead>
      <tr>
        <th class="text-uppercase">Desserts(100g Servings)</th>
        <th class="text-uppercase text-center">calories</th>
        <th class="text-uppercase text-center">Fat(g)</th>
        <th class="text-uppercase text-center">Carbs(g)</th>
        <th class="text-uppercase text-center">protein(g)</th>
      </tr>
    </thead>

    <tbody>
      <tr v-for="item in desserts" :key="item.dessert">
        <td>
          {{ item.dessert }}
        </td>
        <td class="text-center">
          {{ item.calories }}
        </td>
        <td class="text-center">
          {{ item.fat }}
        </td>
        <td class="text-center">
          {{ item.carbs }}
        </td>
        <td class="text-center">
          {{ item.protein }}
        </td>
      </tr>
    </tbody>
  </VTable>
</template>

