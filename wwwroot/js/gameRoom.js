$(document).ready(function () {
    FetchGameData();
    setInterval(InitializeNewRoundTimer, 5000);
    //StartRoundTimer
    //GetLastUnansweredRound
    //BuildRowWithButtonsFromUnansweredRound
});
//WORKFLOW
//You enter, the game up to now gets loaded - done
//timer get initialized
//newest round gets loaded with buttons for answering


//TODO : last, need to redraw table

function InitializeNewRoundTimer() {
    //tuka treba da se refreshira databazata/ da se povika od novo za da se vcitaat 
    console.log("initialize");
    GetResultFromCurrentRound();

    //get the newest round
    const url = "https://localhost:44386/Game/GetNewestRound?roomName=" + "test room";//${ roomName };
    $.ajax({
        url: url,
        method: 'GET',
        success: function (data) {
            AddRowForAnswering(data);
            //table.row.add(rowData).draw(false);
        },
        error: function (xhr, status, error) {
            console.error('There was a problem with the AJAX request:', error);
        }
    });
}

function GetResultFromCurrentRound() {
    console.log("GetResultFromCurrentRound");
    debugger;
    var table = $('#questionsTable').DataTable();
    var lastRowIndex = table.rows().count() - 1;
    var newCellValue = 'new value';//TODO
    table.cell(lastRowIndex, -1).data(newCellValue);
}

function FetchGameData() {
    $('#questionsTable').DataTable({
        ajax: {
            url: "Game/GetQuestionsTable",
            type: "POST",
            dataSrc: function (response) {
                debugger;
                if (response != null && response != undefined) {
                    return response.map(function (item) {
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
        paginate: false,
        filter: false,
        info: false,
        columns: [
            { data: "roundNumber", name: "roundNumber" },
            { data: "expression", name: "expression" },
            { data: "yourAnswer", name: "yourAnswer" },
            { data: "result", name: "result" },
        ],
        initComplete: function () {
            $("#loadingComponent").hide();
            InitializeNewRoundTimer();
        },
        columnDefs: [
            {
                targets: '_all', // Apply to all columns
                defaultContent: '' // Default content for missing columns
            }
        ]
    });
}

function AddRowForAnswering(data) {
    var buttonsForAnsweringHtml = ` <button class="answer-button" onclick="javascript: SubmitAnswer(` + data.roundNumber + `, 'Yes')">Yes</button>
                                    <button class="answer-button" onclick="javascript: SubmitAnswer(` + data.roundNumber + `, 'No')">No</button>`;

    var table = $('#questionsTable').DataTable();
    var lastRowIndex = table.rows().count() - 1;
    table.cell(lastRowIndex, -2).data(buttonsForAnsweringHtml);

        $('.answer-button').on('click', function () {
        var roundNumber = $(this).data('round-number');
        var answer = $(this).text();
        SubmitAnswer(roundNumber, answer);
    });
}

function LoadGameData(dataArray) {
    var data = dataArray;
    console.log(data);
    console.log("LoadGameData");
    $('#questionsTable').DataTable({
        dataSrc: function (data) {
            if (data !== null && data !== undefined) {
                return data.map(function (item) {
                    console.log(item["roundNumber"], item["expression"]);
                    var answered = "";
                    if (item["yourAnswer"] === undefined || item["yourAnswer"] === null) {
                        answered === " Did not participate"
                    }
                    return {
                        "roundNumber": item["roundNumber"],
                        "expression": item["expression"],
                        "yourAnswer": answered,
                        "result": "TODO"
                    };
                });
            }
        },
        serverSide: true,
        paginate: false,
        filter: false,
        info: false,
        columns: [
            { data: "roundNumber", name: "roundNumber" },
            { data: "expression", name: "expression" },
            { data: "yourAnswer", name: "yourAnswer" },
            { data: "result", name: "result" },
        ],
        columnDefs: [
            {
                targets: '_all',
                defaultContent: ''
            }
        ]
    });
}

function SubmitAnswer(roundNumber, answer) {
    //TODO : read values from input fileds
    //set answered cell data
    //submit answer

    SetYourAnswerCellText(answer);

    //TODO : round number is 0 here, fix

    var data = {
        Answer: answer,
        RoundNumber: roundNumber,
        Timestamp: Date.now(),
        RoomName: "test room",
    };
    debugger;
    //TODO : roomname is null
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

function SetYourAnswerCellText(answer) {
    debugger;
    //get the result from current round
    var table = $('#questionsTable').DataTable();
    var lastRowIndex = table.rows().count() - 1;
    var newCellValue = answer;
    table.cell(lastRowIndex, -2).data(newCellValue);
}