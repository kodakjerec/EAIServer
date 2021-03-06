angular.module('starter.controllers')
.controller('ISHIDA_SFD001Ctrl', function ($scope, $ionicLoading, $ionicPopup, $http) {
    $scope.search = {
        DEVICE_AREA: 'A3W1',
        CALLING_NUM: '',
        Division: '',
        Class: '',
        Shop: ''
    };
    $scope.query = function (Mode, queryType) {

        var StartSeq = 0;
        var MaxLength = 20;
        if ($scope.SFD001 != undefined) {
            MaxLength = $scope.SFD001.length;
            if (MaxLength != 0)
                StartSeq = parseInt($scope.SFD001[MaxLength - 1].Field10) + 1;
            else {
                MaxLength = 20;
                StartSeq = 0;
            }
        }
        else
            StartSeq = parseInt('0');

        if (queryType != undefined) {
            switch (queryType) {
                case 'previous':
                    if(StartSeq>0)
                        StartSeq = parseInt($scope.SFD001[0].Field10) - MaxLength;
                    break;
                case 'next':
                default:
                    break;
            }
        }

        var msg = $ionicLoading.show({
            template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
        });
        $http({
            method: 'POST',
            url: 'handler1.ashx',
            params: {
                mode: 'queryISHIDA',
                type: Mode,
                MaxLength: MaxLength,
                StartSeq: StartSeq,
                Shop: $scope.search.Shop,
                CALLING_NUM: $scope.search.CALLING_NUM,
                DEVICE_AREA: $scope.search.DEVICE_AREA
            },
            responseType: "application/json"
        })
        .success(function (response) {
            console.log(response);
            if (response.RT_CODE > 0) {
                $ionicPopup.alert({
                    title: '錯誤',
                    template: response.RT_MSG
                });
            }
            else {
                $scope.SFD001 = response.RT_MSG
            }

        })
        .error(function (response) {
            console.log(response);
        })
        .finally(function () {
            $ionicLoading.hide();
        })
        ;
    };
})
;