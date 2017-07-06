angular.module('starter.directives', ['ionic'])
.directive('ngEnter', function ($rootScope) {
    return function (scope, element, attrs) {
        element.bind("keyup", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
})
.directive('focusMe', function ($timeout) {
    return {
        link: function (scope, element, attrs) {
            $timeout(function () {
                element[0].focus();
            }, 500);
        }
    };
});