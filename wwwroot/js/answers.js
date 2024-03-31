var isFirstRound = true;
function AddRowForAnswering(data) {
    var roundNumber = (data.roundNumber - 1).toString();
    var buttonsForAnsweringHtml = ` <button class="answer-button" onclick="javascript: SubmitAnswer(` + roundNumber + `, 'Yes')">Yes</button>
                                    <button class="answer-button" onclick="javascript: SubmitAnswer(` + roundNumber + `, 'No')">No</button>`;

    var table = $('#questionsTable').DataTable();
    var lastRowIndex = table.rows().count() - 1;
    table.cell(lastRowIndex, -2).data(buttonsForAnsweringHtml);
}

function RemoveOldButtons(data) {
    if (data.result === "" && !isFirstRound) {
        var table = $('#questionsTable').DataTable();
        var lastRowIndex = table.rows().count() - 2;
        table.cell(lastRowIndex, -2).data("MISSED");
        table.cell(lastRowIndex, -1).data("FAILED");
    } else {
        isFirstRound = false;
    }
}

function AddAnswerToTableArray(roundNumber, answer, response) {
    var newRowData = tableRoundsArray[roundNumber - 1];
    if (response === 1) {
        newRowData.result = "CORRECT";
        newRowData.answer = answer;
        playerScore++;

        $("#playerScore").text(playerScore);
    }
    if (response === 0) {
        newRowData.result = "MISSED";
        newRowData.answer = answer;
    }
    if (response === -1) {
        newRowData.result = "FAILED";
        newRowData.answer = answer;
    }
    tableRoundsArray[roundNumber - 1] = newRowData;
}

function SubmitAnswer(roundNumber, answer) {
    SetYourAnswerCellText(answer);
    var data = {
        Answer: answer,
        RoundNumber: roundNumber,
        Timestamp: Date.now(),
        RoomName: "test room",
    };
    $.ajax({
        type: "POST",
        url: "Game/SubmitAnswer",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response !== null && response !== undefined) {
                AddAnswerToTableArray(roundNumber, answer, response);
            } else {
                alert("We are experiencing difficulties at the moment, please try again later");
            }
        },
        error: function (xhr, status, error) {
            alert("We are experiencing difficulties at the moment, please try again later");
        }
    });
}

function SetYourAnswerCellText(answer) {
    var table = $('#questionsTable').DataTable();
    var lastRowIndex = table.rows().count() - 1;
    var newCellValue = answer;
    table.cell(lastRowIndex, -2).data(newCellValue);
}