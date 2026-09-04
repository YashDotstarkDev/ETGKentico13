<script lang="ts">
import {defineComponent} from 'vue'

export default defineComponent({
  name: "NavController",
  data() {
    return {
      submenus: []
    }
  },
  methods: {
    updateMegamenus() {
      // set the height of each megamenu to the height of the highest child ul
      document.querySelectorAll('.megamenu').forEach((megamenu) => {
        let maxHeight = 0
        megamenu.querySelectorAll('ul').forEach((ul) => {
          if (ul.getBoundingClientRect().height > maxHeight) {
            maxHeight = ul.getBoundingClientRect().height
          }
        })
        megamenu.querySelectorAll('.cta').forEach((ul) => {
          if (ul.getBoundingClientRect().height > maxHeight) {
            maxHeight = ul.getBoundingClientRect().height
          }
        })
        megamenu.style.height = maxHeight + 80 + 'px'
      })
    }
  },
  mounted() {

    this.updateMegamenus()

    // on resize
    window.addEventListener('resize', () => {
      this.updateMegamenus()
    })

    // on click anywhere in the document
    document.addEventListener('click', (e) => {
      // if the click target doesn't have a parent with the class .menu-item
      if (!e.target.closest('.menu-item') && !e.target.closest('.megamenu > *')) {
        // remove all active classes
        document.querySelectorAll('header .menu-group').forEach((menu) => {
          menu.classList.remove('menu-item-active')
        })
        // hide all submenus
        this.submenus.forEach((submenu) => {
          submenu.submenu.classList.remove('pointer-events-auto','opacity-100','visible','delay-0')
          submenu.submenu.classList.add('pointer-events-none','opacity-0','invisible','delay-150')
        })
      }
    })

    // for each .top-bar .menu-item
    // document.querySelectorAll('.top-bar .menu-item').forEach((menuItem) => {
    //   // if the menuItem has a sibling .submenu
    //   if (menuItem.nextElementSibling && menuItem.nextElementSibling.classList.contains('submenu')) {
    //     const submenu = menuItem.nextElementSibling
    //     const parentMenu = menuItem.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement
    //
    //     // move the submenu out to the parent .main-menu
    //     parentMenu.appendChild(submenu)
    //
    //     // set the left of the submenu to the left of the menuItem
    //     submenu.style.left = menuItem.getBoundingClientRect().left + 'px'
    //     // set the top of the submenu to the bottom of the menuItem
    //     submenu.style.top = menuItem.getBoundingClientRect().bottom + 8 + 'px'
    //
    //     // set the width to 20px wider than it currently is
    //     submenu.style.width = submenu.getBoundingClientRect().width + 40 + 'px'
    //
    //     // on menuItem click
    //     menuItem.addEventListener('click', (e) => {
    //       // if the menuItem is not active
    //       if (!menuItem.parentElement.classList.contains('menu-item-active')) {
    //         // remove all active classes
    //         document.querySelectorAll('header .menu-group').forEach((menu) => {
    //           menu.classList.remove('menu-item-active')
    //         })
    //         // hide all submenus
    //         this.submenus.forEach((submenu) => {
    //           submenu.submenu.classList.remove('pointer-events-auto','opacity-100','visible','delay-0')
    //           submenu.submenu.classList.add('pointer-events-none','opacity-0','invisible','delay-150')
    //         })
    //         // add active class to the menuItem
    //         menuItem.parentElement.classList.add('menu-item-active')
    //         submenu.classList.add('pointer-events-auto','opacity-100','visible')
    //         submenu.classList.remove('pointer-events-none','opacity-0','invisible')
    //       } else {
    //         // remove active class from the menuItem
    //         menuItem.parentElement.classList.remove('menu-item-active')
    //         submenu.classList.remove('pointer-events-auto','opacity-100','visible')
    //         submenu.classList.add('pointer-events-none','opacity-0','invisible')
    //       }
    //     })
    //
    //     this.submenus.push({
    //       submenu: submenu,
    //       menuItem: menuItem
    //     })
    //   }
    // })


    // resize the main nav submenus


    // for each header nav .menu-item
    // REMOVING CLICK TOGGLE FUNCTION
    // document.querySelectorAll('header nav .menu-item').forEach((menuItem) => {
    //   // if the menuItem has a sibling .submenu or a sibling .megamenu
    //   if (menuItem.nextElementSibling && (menuItem.nextElementSibling.classList.contains('submenu') || menuItem.nextElementSibling.classList.contains('megamenu'))) {
    //     menuItem.addEventListener('click', (e) => {
    //       if (!menuItem.parentElement.classList.contains('menu-item-active')) {
    //         document.querySelectorAll('header .menu-group').forEach((menuGroup) => {
    //           menuGroup.classList.remove('menu-item-active')
    //           this.submenus.forEach((submenu) => {
    //             submenu.submenu.classList.remove('pointer-events-auto','opacity-100','visible','delay-0')
    //             submenu.submenu.classList.add('pointer-events-none','opacity-0','invisible','delay-150')
    //           })
    //         })
    //         menuItem.parentElement.classList.add('menu-item-active')
    //       } else {
    //         menuItem.parentElement.classList.remove('menu-item-active')
    //       }
    //     })
    //   }
    // })

    // for each header nav .menu-primary
    document.querySelectorAll('header nav div.menu-primary').forEach((menuItem) => {
      menuItem.addEventListener('click', (e) => {
        if (!menuItem.parentElement.classList.contains('menu-primary-active')) {

          menuItem.parentElement.parentElement.parentElement.querySelectorAll('.menu-primary-group').forEach((menuGroup) => {
            menuGroup.classList.remove('menu-primary-active')

          })

          // This removes active from all other secondary items
          menuItem.parentElement.parentElement.querySelectorAll('.menu-secondary-group').forEach((menuGroup) => {
            menuGroup.classList.remove('menu-secondary-active')
          })

          menuItem.parentElement.classList.add('menu-primary-active')

          // add class to the first menu-secondary-group
          menuItem.parentElement.querySelector('.menu-secondary-group').classList.add('menu-secondary-active')
        } else {
          // menuItem.parentElement.classList.remove('menu-primary-active')
        }
      })
    })

    // for each header nav .menu-secondary
    document.querySelectorAll('header nav div.menu-secondary').forEach((menuItem) => {
      menuItem.addEventListener('click', (e) => {
        if (!menuItem.parentElement.classList.contains('menu-secondary-active')) {

          menuItem.parentElement.parentElement.parentElement.querySelectorAll('.menu-secondary-group').forEach((menuGroup) => {
            menuGroup.classList.remove('menu-secondary-active')
          })
          // This removes active from all other secondary items
          // document.querySelectorAll('header nav .menu-secondary-group').forEach((menuGroup) => {
          //   menuGroup.classList.remove('menu-secondary-active')
          // })
          menuItem.parentElement.classList.add('menu-secondary-active')
        } else {
          // menuItem.parentElement.classList.remove('menu-secondary-active')
        }
      })
    })

    let groupTimers = []
    // for each .menu-group
    document.querySelectorAll('header .menu-group').forEach((menuGroup, index) => {
      groupTimers.push(null)

      // add a class to this div on hover
      menuGroup.addEventListener('mouseover', (e) => {
        // clear the timeout
        if (groupTimers[index]) {
          clearTimeout(groupTimers[index])
        }
        menuGroup.classList.add('group-active')
      })

      // remove the class on mouseout
      menuGroup.addEventListener('mouseout', (e) => {
        // in half a second
        groupTimers[index] = setTimeout(() => {
          menuGroup.classList.remove('group-active')
        }, 250)
      })
    })
  }
})
</script>

<template>
<div></div>
</template>

<style scoped lang="scss">

</style>
