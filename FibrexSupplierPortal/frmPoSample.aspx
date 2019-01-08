<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPoSample.aspx.cs" Inherits="FibrexSupplierPortal.frmPoSample" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="Content/handsontable.full.min.css" /> <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="Scripts/handsontable.full.min.js"></script>


    <script data-jsfiddle="common">
     
  </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
        <div>
       <%--       <div id="example" style="margin: 20px;"></div>--%>

         <script data-jsfiddle="example">
                function example(dt) {
                    //falert(dt);
                   // var data = [{ "ID": 1, "bookingID": 1, "Schema": "10:00 Welcom" }, { "ID": 2, "bookingID": 1, "Schema": "12:00 Lunch" }, { "ID": 3, "bookingID": 1, "Schema": "15:00 coffee break" }, { "ID": "s", "bookingID": null, "Schema": null }, { "ID": null, "bookingID": null, "Schema": null }]
                    var data = dt;
                    var $container = $("#example");
                    var $parent = $container.parent(); 
                    $container.handsontable({
                       data: data, 
                        startRows: 3,
                        startCols: 7,
                        rowHeaders: true,
                        manualRowMove: true,
                        colHeaders: ['Cost Code', 'Line Type', 'Description', 'Qtn', 'Unit', 'Unit Price', 'Total Price'],
                        
                        
                        minSpareRows: 1,
                        contextMenu: true
                    });

                    var handsontable = $container.data('handsontable');
                    $parent.find('input[name=btnSave]').click(function () {
                        $("#tableData").val(JSON.parse(handsontable.getData()));
                    });

                    //$("#example").handsontable({
                    //$("#tableData").val(JSON.stringify(handsontable.getData()));
                    //    colHeaders: ["PONUM", "LINETYPE", "COSTCODE","ITEMDESCRIPTION","ORDERQTY","UNITCOST","LINECOST"],
                    //});
                }
            </script> 
            <asp:HiddenField ID="tableData" runat="server" />
            <asp:Button ID="btnSave" runat="server" Text="Load Records" OnClick="btnSave_Click" Visible="true" />

            Get from Grid: <br />
            <asp:Label ID="lblDataShow" runat="server"></asp:Label>

            <asp:GridView ID="example" runat="server"></asp:GridView>
        </div>
    </form>
</body>
</html>
