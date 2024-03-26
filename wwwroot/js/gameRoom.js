$(document).ready(function () {
    LoadGameData();
    setInterval(InitializeNewRoundTimer, 50000);
    //StartRoundTimer
    //GetLastUnansweredRound
    //BuildRowWithButtonsFromUnansweredRound
});
//WORKFLOW
//You enter, the game up to now gets loaded - done
//timer get initialized
//newest round gets loaded with buttons for answering

function InitializeNewRoundTimer() {
    const url = "https://localhost:44386/Game/GetNewestRound?roomName=" + "test room";//${ roomName };

    $.ajax({
        url: url,
        method: 'GET',
        success: function (data) {
            // Process the received data
            //tuka treba da se izgradi redot za input
            AddRowForAnswering(data);
        },
        error: function (xhr, status, error) {
            console.error('There was a problem with the AJAX request:', error);
        }
    });
}

function AddRowForAnswering(data) {
    debugger;
    var rowHTML = `
    <tr class="even">
        <td>` + data.roundNumber + `</td>
        <td>` + data.expression + `</td>
        <td>
            <button onclick="javascript: SubmitAnswer(` + data.roundNumber + `, Yes)">Yes</button>
            <button onclick="javascript: SubmitAnswer(` + data.roundNumber + `, No)">No</button>
        </td>
        <td></td>
    </tr>
`;

    $('#questionsTable').DataTable().row.add($(rowHTML)).draw();
}

function AnsweredYes() {
    const url = "https://localhost:44386/Game/SubmitAnswer?roomName=" + "test room";//${ roomName };
    var obj = {
        answer: 'yes',
        roundNumber: 1,
        roomName: "test room",
        timestamp: Date.now()
    }

    $.ajax({
        url: url,
        method: 'POST',
        object: JSON.stringify(obj),
        success: function (data) {
            // Process the received data
            //tuka treba da se izgradi redot za input
            AddRowForAnswering(data);
        },
        error: function (xhr, status, error) {
            console.error('There was a problem with the AJAX request:', error);
        }
    });
}

function AnsweredNo() {

}

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
        paginate: false,
        filter: false,
        info: false,
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

function SubmitAnswer(roundNumber, answer) {
    //TODO : read values from input fileds
    var data = {
        Answer: answer,
        RoundNumber: roundNumber,
        Timestamp: Date.now(),
    };
    debugger;
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