function KeyUpHandler(sender) { }

function KeyDownHandler(maskExtenderId) {

    if (navigator.appName != "Microsoft Internet Explorer") {



        if (event.keyCode == 35 || event.keyCode == 36) { // Home and End buttons functionality



            var txtElement = $get(event.srcElement.id);

            var txtElementText = GetTextElementValue(event.srcElement.id);



            if (event.keyCode == 36) {//Home button

                setCaretPosition(txtElement, 0);

            }

            if (event.keyCode == 35) {//End button

                setCaretPosition(txtElement, txtElementText.length);

            }

        }


        if (event.keyCode == 8 || event.keyCode == 46) {



            var txtElement = $get(event.srcElement.id);

            var txtElementText = GetTextElementValue(event.srcElement.id);

            var txtElementCursorPosition = doGetCaretPosition(txtElement);

            var maskExtender = $find(maskExtenderId);


            var start = txtElement.selectionStart;

            var end = txtElement.selectionEnd;

            var selectedSymbols = end - start;



            if (event.keyCode == 8) //BackSpace
            {

                if (selectedSymbols > 0) {//if there is selection(more then 1 symbol)


                    var str1 = txtElementText.substr(0, start);

                    var str2 = txtElementText.substr(end);

                    var str = str1 + str2;

                    if (str.length < txtElementText.length) str = appendStrWithChar(str, txtElementText, "_");

                    SetTextElementValue(event.srcElement.id, str);

                    //txtElement.value = str;

                    maskExtender._LogicTextMask = deletePromptChars(str, "_");

                    setCaretPosition(txtElement, start);

                }

                else {

                    if ((txtElementCursorPosition - 1) >= 0) {

                        var symbol_to_delete = txtElementText[txtElementCursorPosition - 1];

                        if (symbol_to_delete == "_") {

                            setCaretPosition(txtElement, txtElementCursorPosition - 1);

                        }

                        else {

                            var str1 = txtElementText.substr(0, txtElementCursorPosition - 1);

                            var str2 = txtElementText.substr(txtElementCursorPosition);

                            var str = str1 + str2;

                            if (str.length < txtElementText.length) str = appendStrWithChar(str, txtElementText, "_");

                            SetTextElementValue(event.srcElement.id, str);

                            //txtElement.value = str;

                            maskExtender._LogicTextMask = deletePromptChars(str, "_");

                            setCaretPosition(txtElement, txtElementCursorPosition - 1);

                            //var real_text = deletePromptChars(str, "_");

                        }

                    }

                }



            }

            if (event.keyCode == 46) //Delete
            {

                if (txtElementCursorPosition >= 0 && txtElementCursorPosition < txtElementText.length

                        && ((selectedSymbols <= 1 && txtElementText[txtElementCursorPosition] != "_") || selectedSymbols > 1)) {


                    if (selectedSymbols > 1) {//if there is selection(more then 1 symbol)

                        var str1 = txtElementText.substr(0, start);

                        var str2 = txtElementText.substr(end);

                        var str = str1 + str2;

                        if (str.length < txtElementText.length) str = appendStrWithChar(str, txtElementText, "_");

                        SetTextElementValue(event.srcElement.id, str);

                        //txtElement.value = str;

                        maskExtender._LogicTextMask = deletePromptChars(str, "_");

                        setCaretPosition(txtElement, start);

                    }

                    else {//no selection or 1 symbol selected

                        var symbol_to_delete = txtElementText[txtElementCursorPosition];


                        if (symbol_to_delete != "_") {

                            var str1 = txtElementText.substr(0, txtElementCursorPosition);

                            var str2 = txtElementText.substr(txtElementCursorPosition + 1);

                            var str = str1 + str2;

                            if (str.length < txtElementText.length) str = appendStrWithChar(str, txtElementText, "_");

                            SetTextElementValue(event.srcElement.id, str);

                            //txtElement.value = str;

                            maskExtender._LogicTextMask = deletePromptChars(str, "_");

                            setCaretPosition(txtElement, txtElementCursorPosition);

                        }

                    }

                }



            }

        }


    }


}

function GetTextElementValue(elementId) {

    var textBox = $get(elementId), text;

    if (textBox.AjaxControlToolkitTextBoxWrapper) {

        text = textBox.AjaxControlToolkitTextBoxWrapper.get_Value();

    }

    else {

        text = textBox.value;

    }


    return text;

}


function SetTextElementValue(elementId, someText) {

    var textBox = $get(elementId);

    if (textBox.AjaxControlToolkitTextBoxWrapper) {

        textBox.AjaxControlToolkitTextBoxWrapper.set_Value(someText);

    }

    else {

        textBox.value = someText;

    }

}


function appendStrWithChar(str, templateStr, appChar) {

    var newStr = str;

    var difference = templateStr.length - newStr.length;


    if (difference > 0) {

        for (i = 0; i < difference; i++) { newStr = newStr + "_"; }

    }

    return newStr;

}


function deletePromptChars(str, promptChar) {

    var newStr = str;

    for (i = 0; i < newStr.length; i++) {

        if (str[i] == promptChar) {

            newStr = newStr.substr(0, i);

            return newStr;

        }

    }

}


function doGetCaretPosition(ctrl) {

    var CaretPos = 0; // IE Support

    if (document.selection) {

        ctrl.focus();

        var Sel = document.selection.createRange();

        Sel.moveStart('character', -ctrl.value.length);

        CaretPos = Sel.text.length;

    }

        // Firefox support

    else if (ctrl.selectionStart || ctrl.selectionStart == '0')

        CaretPos = ctrl.selectionStart;

    return (CaretPos);

}


function setCaretPosition(ctrl, pos) {

    if (ctrl.setSelectionRange) {

        ctrl.focus();

        ctrl.setSelectionRange(pos, pos);

    }

    else if (ctrl.createTextRange) {

        var range = ctrl.createTextRange();

        range.collapse(true);

        range.moveEnd('character', pos);

        range.moveStart('character', pos);

        range.select();

    }

}
