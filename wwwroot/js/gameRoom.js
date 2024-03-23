$(document).ready(function () {
    LoadGameData();
});

function LoadGameData() {
    $('#questionsTable').DataTable({
        ajax: {
            url: "Game/GetQuestionsTable",
            type: "POST",
            dataSrc: function (response) {

                if (response != null && response != undefined) {
                    // Modify the response data or handle it as needed
                    return response.map(function (item) {
                        // Exclude additional fields from each item in the response
                        return {
                            "roundNumber": item["roundNumber"],
                            "expression": item["expression"],
                            "yourAnswer": item["yourAnswer"],
                            "result": item["v"]
                        };
                    });
                } else {
                    //TODO : custom alert
                    alert("We are experiencing difficulties at the moment, please try again later");
                }
            }
        },
        serverSide: true,
        columns: [
            { data: "roundNumber", name: "roundNumber" },
            { data: "expression", name: "expression" },
            { data: "yourAnswer", name: "yourAnswer" },
            { data: "result", name: "result" },
        ]
    });
}

function BuildInitialTable() {
    //TODO : here build the initial table
}

function RefreshGameTable() {

}

function SubmitAnswer() {
    //TODO : read values from input fileds
    var data = {
        Answer: "Yes",
        RoundNumber: 1,
        RoomName: "Blue room",
        Timestamp: Date.now(),
    };
    $.ajax({
        type: "POST",
        url: "Game/SubmitAnswer",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (data, status) {
            alert("Data: " + data + "\nStatus: " + status);
        },
        error: function (xhr, status, error) {
            alert("Error: " + error);
        }
    });
}