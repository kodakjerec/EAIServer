// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
// 'starter.controllers' is found in controllers.js
angular.module('starter', ['ionic', 'starter.controllers', 'starter.services','starter.directives'])

.run(function ($ionicPlatform) {
    //$ionicPlatform.ready(function () {
    //    // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
    //    // for form inputs)
    //    if (cordova.platformId === "ios" && window.cordova && window.cordova.plugins.Keyboard) {
    //        cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
    //        cordova.plugins.Keyboard.disableScroll(true);

    //    }
    //    if (window.StatusBar) {
    //        // org.apache.cordova.statusbar required
    //        StatusBar.styleDefault();
    //    }
    //});
})
.config(function ($stateProvider, $urlRouterProvider) {
    $stateProvider
    .state('menu', {
        url: '/menu',
        cache: false,
        templateUrl: 'templates/menu.html',
        controller: 'menuCtrl'
    })
    .state('menu.001', {
        url: '/001',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/CheckSettings.html',
                controller: '001Ctrl'
            }
        }
    })
    .state('ISHIDA', {
        url: '/ISHIDA',
        cache: false,
        templateUrl: 'templates/menu_ISHIDA.html'
    })
    .state('ISHIDA.SFD001', {
        url: '/SFD001',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/ISHIDA_SFD001.html',
                controller: 'ISHIDA_SFD001Ctrl'
            }
        }
    })
    .state('ISHIDA.SFM001', {
        url: '/SFM001',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/ISHIDA_SFM001.html',
                controller: 'ISHIDA_SFM001Ctrl'
            }
        }
    })
    .state('ISHIDA.SFM002', {
        url: '/SFM002',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/ISHIDA_SFM002.html',
                controller: 'ISHIDA_SFM002Ctrl'
            }
        }
    })
;
    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/ISHIDA/SFD001');
});