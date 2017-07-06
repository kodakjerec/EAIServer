angular.module('starter.services', [])
.factory('_11_CheckIn', function ($rootScope, $http, $ionicLoading, $ionicPopup, $timeout) {
    return {
        //查詢報到車號
        QueryCarNo: function (param_CarNo) {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_CarNo.ashx',
                params: { mode: 'Query', CarNo: param_CarNo }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        },
        //查詢報到車號驗收明細
        QueryPaperDetail: function (param_CarNo) {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_CarNo.ashx',
                params: { mode: 'QueryPaperDetail', CarNo: param_CarNo }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        },
        //車號報到
        CarCheck: function (param_CarNo, param_TEMP0, param_TEMP1, param_TEMP2, param_UserName) {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_CarNo.ashx',
                params: { mode: 'CarCheck', CarNo: param_CarNo, TEMP0: param_TEMP0, TEMP1: param_TEMP1, TEMP2: param_TEMP2, USER_ID: param_UserName }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        }
    }
})
.factory('_12_PaperNo', function ($rootScope, $http, $ionicLoading, $ionicPopup, $timeout) {
    return {
        //查詢進貨單
        Query: function (param_CarNo, param_PaperNo, param_UserID) {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_PaperNo.ashx',
                params: { mode: 'Query', CarNo: param_CarNo, PaperNo: param_PaperNo, USER_ID: param_UserID }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        },
        //查詢進貨單驗收明細
        QueryPaperDetail: function (param_CarNo, param_PaperNo) {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_PaperNo.ashx',
                params: { mode: 'QueryPaperDetail', CarNo: param_CarNo, PaperNo: param_PaperNo }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        },
        //完成驗收
        Finish: function (param_CarNo, param_PaperNo, param_IDNo) {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_PaperNo.ashx',
                params: { mode: 'FinishPaperNo', CarNo: param_CarNo, IDNo: param_IDNo }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        }
    }
})
.factory('_12_ItemCode', function ($rootScope, $http, $ionicLoading, $ionicPopup, $timeout) {
    return {
        //查詢商品條碼
        Query: function (param_IDNo, param_ItemCode, param_UserID) {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_ItemCode.ashx',
                params: { mode: 'Query', PaperNo: param_IDNo, ItemCode: param_ItemCode, USER_ID: param_UserID }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        },
        //開始驗收(鎖定)
        JOBID2: function (param_IDNo, param_HOID, param_LOT, param_SunDay, param_Temp, param_UserID) {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_ItemCode.ashx',
                params: { mode: 'JOBID2', PaperNo: param_IDNo, HOID: param_HOID, LOT: param_LOT, SunDay: param_SunDay, Temp: param_Temp, USER_ID: param_UserID }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        },
        //單品完成
        JOBID45: function (param_IDNo, param_HOID, param_UserID) {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_ItemCode.ashx',
                params: { mode: 'JOBID45', IDNo: param_IDNo, HOID: param_HOID, USER_ID: param_UserID }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        }
    }
})
.factory('_12_ItemReceive', function ($rootScope, $http, $ionicLoading, $ionicPopup, $timeout) {
    return {
        //選品質
        QueryItemState: function () {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_ItemReceive.ashx',
                params: { mode: 'QueryItemState' }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        },
        //驗收
        Receive: function (param_IDNo, param_LOTID, param_State, param_QTY, param_WT, param_UserID) {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_ItemReceive.ashx',
                params: { mode: 'Receive', IDNo: param_IDNo, LockNo: param_LOTID, TYPE: param_State, QTY: param_QTY, WT: param_WT, USER_ID: param_UserID }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        },
        //解鎖
        UnLock: function (param_IDNo, param_LOTID, param_UserID) {
            $ionicLoading.show({
                template: '<p>查詢中...</p><ion-spinner></ion-spinner>'
            });
            return $http({
                method: 'GET',
                url: 'http://' + $rootScope.ServerIP + '/RF_Web/handler/12_ItemReceive.ashx',
                params: { mode: 'UnLock', IDNo: param_IDNo, LockNo: param_LOTID, USER_ID: param_UserID }
            })
            .success(function (response) {
                return response;
            })
            .error(function (response) {
                var msg = $ionicPopup.alert({
                    title: '錯誤',
                    template: '無法送出查詢'
                });

                $timeout(function () {
                    msg.close(); //close the popup after 3 seconds for some reason
                }, $rootScope.Set_timeout);
                return '';
            })
            .finally(function () {
                $ionicLoading.hide();
            });
        }
    }
})
;