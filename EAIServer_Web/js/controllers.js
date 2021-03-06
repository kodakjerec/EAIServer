angular.module('starter.controllers', [])
.controller('menuCtrl', function () {

})
.controller('001Ctrl', function ($scope, $ionicLoading, $ionicPopup, $http) {
    $scope.data = {
        clickedButton: 0
    }

    $scope.isClicked = function (myIndex) {
        if (myIndex == $scope.data.clickedButton)
            return 'button-calm'
        else
            return ''
    };

    $scope.query = function (Mode) {
        $scope.schedule = undefined;
        $scope.database = undefined;
        $scope.agent = undefined;
        $scope.command = undefined;
        $scope.schedulelist = undefined;

        var msg = $ionicLoading.show({
            template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
        });
        $http({
            method: 'POST',
            url: 'handler1.ashx',
            params: { mode: 'queryTAKO', type:Mode},
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
                switch (Mode) {
                    case 'schedule':
                        $scope.data.clickedButton = 0;
                        $scope.schedule = response.RT_MSG;
                        break;
                    case 'database':
                        $scope.data.clickedButton = 1;
                        $scope.database = response.RT_MSG;
                        break;
                    case 'agent':
                        $scope.data.clickedButton = 2;
                        $scope.agent = response.RT_MSG;
                        break;
                    case 'command':
                        $scope.data.clickedButton = 3;
                        $scope.command = response.RT_MSG;
                        break;
                    case 'schedulelist':
                        $scope.data.clickedButton = 4;
                        $scope.schedulelist = response.RT_MSG;
                        break;
                }
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

    $scope.gotop = function () {
        $("html.body").animate({
            scrollTop: 0
        },1000);
    };

    $scope.DoTAKOcommand = function (command) {
        $scope.data.TAKOcommand = {
            command: command,
            args1: '',
            args2: ''
        };
        $scope.data.TAKOcommand.command=command;
        // An elaborate, custom popup
        var myPopup = $ionicPopup.show({
            template: '命令：<b>'+command+'</b><br/>@args1: <input type="text" ng-model="data.TAKOcommand.args1"><br/>@args2: <input type="text" ng-model="data.TAKOcommand.args2">',
            title: '輸入其他參數',
            subTitle: '例如:@args1, @args2',
            scope: $scope,
            buttons: [
              { text: '取消' },
              {
                  text: '<b>送出</b>',
                  type: 'button-positive',
                  onTap: function (e) {
                      if (!$scope.data.TAKOcommand) {
                          //don't allow the user to close unless he enters wifi password
                          e.preventDefault();
                      } else {
                          var sendCommand = $scope.data.TAKOcommand.command + ' "' + $scope.data.TAKOcommand.args1 + '" "' + $scope.data.TAKOcommand.args2+'" '

                          $http({
                              method: 'POST',
                              url: 'handler1.ashx',
                              params: { mode: 'DoTAKOcommand', command:sendCommand },
                              responseType: "application/json"
                          })
                      }
                  }
              }
            ]
        });
    }
})
;