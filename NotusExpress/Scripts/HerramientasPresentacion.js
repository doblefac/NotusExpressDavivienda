/// <reference name="MicrosoftAjax.js"/>

function modifyCalDelegates(cal) {
    //we need to modify the original delegate of the month cell.
    cal._cell$delegates = {
        mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
        mouseout: Function.createDelegate(cal, cal._cell_onmouseout),

        click: Function.createDelegate(cal, function(e) {
            /// <summary> 
            /// Handles the click event of a cell
            /// </summary>
            /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>

            e.stopPropagation();
            e.preventDefault();

            if (!cal._enabled) return;

            var target = e.target;
            var visibleDate = cal._getEffectiveVisibleDate();
            Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
            switch (target.mode) {
                case "prev":
                case "next":
                    cal._switchMonth(target.date);
                    break;
                case "title":
                    switch (cal._mode) {
                        case "days": cal._switchMode("months"); break;
                        case "months": cal._switchMode("years"); break;
                    }
                    break;
                case "month":
                    //if the mode is month, then stop switching to day mode.
                    if (target.month == visibleDate.getMonth()) {
                        //this._switchMode("days");
                    } else {
                        cal._visibleDate = target.date;
                        //this._switchMode("days");
                    }
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
                case "year":
                    if (target.date.getFullYear() == visibleDate.getFullYear()) {
                        cal._switchMode("months");
                    } else {
                        cal._visibleDate = target.date;
                        cal._switchMode("months");
                    }
                    break;

                //                case "day":                              
                //                    this.set_selectedDate(target.date);                              
                //                    this._switchMonth(target.date);                              
                //                    this._blur.post(true);                              
                //                    this.raiseDateSelectionChanged();                              
                //                    break;                              
                case "today":
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }

        })
    }

}

function onCalendarShown(sender, args) {
    //set the default mode to month
    sender._switchMode("months", true);
    changeCellHandlers(cal1);
}


function changeCellHandlers(cal) {

    if (cal._monthsBody) {

        //remove the old handler of each month body.
        for (var i = 0; i < cal._monthsBody.rows.length; i++) {
            var row = cal._monthsBody.rows[i];
            for (var j = 0; j < row.cells.length; j++) {
                $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
            }
        }
        //add the new handler of each month body.
        for (var i = 0; i < cal._monthsBody.rows.length; i++) {
            var row = cal._monthsBody.rows[i];
            for (var j = 0; j < row.cells.length; j++) {
                $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
            }
        }

    }
}

function onCalendarHidden(sender, args) {

    if (sender.get_selectedDate()) {
        if (cal1.get_selectedDate() && cal2.get_selectedDate() && cal1.get_selectedDate() > cal2.get_selectedDate()) {
            alert('La fecha Final no debe ser menor a la fecha Inicial !');
            sender.show();
            return;
        }
        //get the final date
        var finalDate = new Date(sender.get_selectedDate());
        var selectedMonth = finalDate.getMonth();
        finalDate.setDate(1);
        if (sender == cal2) {
            // set the calender2's default date as the last day
            finalDate.setMonth(selectedMonth + 1);
            finalDate = new Date(finalDate - 1);
        }
        //set the date to the TextBox

        // sender.get_element().value = finalDate.format(sender._format); //queda la fecha en ingles !!!
    }
}

