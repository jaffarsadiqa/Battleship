// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var ApiManager = function (host) {    
    var ApiHost = host;
    var ApiToken = null;
    this.ShipList = [];
    this.ShipTypes = ["Carrier", "Cruiser", "Destroyer", "Frigate","Submarine"]
    apiCall = function (method, url, dataToPost, successCallBack, errorCallBack, contentType) {
        $.ajax({
            type: method, //GET, POST, PUT
            url: url,  //the url to call
            data: dataToPost,     //Data sent to server
            contentType: contentType,
            beforeSend: function (xhr) {   //Include the bearer token in header
                xhr.setRequestHeader("Authorization", 'Bearer ' + ApiToken);
            }
        }).done(function (response) {
            //Response ok. process reuslt
            if (successCallBack)
                successCallBack(response);
        }).fail(function (jqXHR, exception) {
            if (jqXHR.status === 400 && jqXHR.responseJSON) {
                alert("Validation Error - "+JSON.stringify(jqXHR.responseJSON.errors))
            }
            //Error during request
            if (errorCallBack)
                errorCallBack(exception)
        });
    }
    var createBoard = function (boardName, successCallBack, failureCallBack) {
        var url = ApiHost + "/Board"
        var postData = JSON.stringify({ "name": boardName });
        success = function (result) {            
            successCallBack(result);
        }
        failure = function (err) {
            console.log(err);
            failureCallBack(err);
            return;
        }
        apiCall("POST", url, postData, success, failure, "application/json");
    }
    this.registerPlayer = function(playerName, successCallBack, failureCallBack){
        var url = ApiHost + "/Login"
        var postData = JSON.stringify({ "name": playerName });
        success = function (result) {
            ApiToken = result.result;
            $('#registerModal').modal('hide')
            console.log(result);
            createBoard(playerName + " Board", successCallBack, failure)
        }
        failure = function (err) {
            console.log(err);
            failureCallBack(err);
            return;
        }
        apiCall("POST", url, postData, success, failure, "application/json");
    }
    this.addShip = function (name, size, type, successCallBack, failureCallBack) {
        var url = ApiHost + "/Ship"
        var postData = JSON.stringify({ "name": name, "boardPointSize": parseInt(size), "type": parseInt(type) });
        apiCall("POST", url, postData, successCallBack, failureCallBack, "application/json");
    }
    this.assignShip = function (shipPlaced, row, col, shipId, successCallBack, failureCallBack) {
        var url = ApiHost + "/Board/AssignShipToBoard?idShip=" + shipId + "&shipAlignType=" + shipPlaced
        var postData = JSON.stringify({ "row": parseInt(row), "column": parseInt(col) });
        apiCall("POST", url, postData, successCallBack, failureCallBack, "application/json");
    }
    this.getShip = function (idShip, successCallBack, failureCallBack) {
        var url = ApiHost + "/Ship?idShip=" + idShip        
        apiCall("GET", url, null, successCallBack, failureCallBack, "application/json");
    }
    this.attackShip = function(row, col, successCallBack, failureCallBack){
        var url = ApiHost + "/Board/AttackShip"
        var postData = JSON.stringify({ "row": parseInt(row), "column": parseInt(col) });
        apiCall("POST", url, postData, successCallBack, failureCallBack, "application/json");
    }
}
var Ship = {
    id: "",
    name: "",
    boardPointSize: 0,
    hitBoardPoints: [],
    shipPlacedNodes: [],
    type: 1
}
var Coordinate = {
    row:0,column:0
}
