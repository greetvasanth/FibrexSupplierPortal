<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="demoReg.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.demoReg" %>

<%@ Register Assembly="DevExpress.Web.v16.1" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/sb-admin-2.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.12.3.js" type="text/javascript"></script>
    <style>
         .show-read-more .more-text{
        display: none;
    }
    </style>
    <script>
        $(document).ready(function () {
            var maxLength = 50;
            $(".show-read-more").each(function () {
                var myStr = $(this).text();
                if ($.trim(myStr).length > maxLength) {
                    var newStr = myStr.substring(0, maxLength);
                    var removedStr = myStr.substring(maxLength, $.trim(myStr).length);
                    $(this).empty().html(newStr);
                    $(this).append(' <a href="javascript:void(0);" class="read-more">read more...</a>');
                    $(this).append('<span class="more-text">' + removedStr + '</span>');
                }
            });
            $(".read-more").click(function () {
                $(this).siblings(".more-text").contents().unwrap();
                $(this).remove();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="txtRegistrationID" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtSupplierName" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Test" OnClientClick="searchvalue();" />
        <%--     <div>  
                <b class="label label-danger" style="padding: 8.5px">Click to Show or Hide Column:</b>  
                <div class="btn-group btn-group-sm">  
                    <a class="showHide btn btn-primary" data-columnindex="0">ID</a>  
                    <a class="showHide btn btn-primary" data-columnindex="1">FirstName</a>  
                    <a class="showHide btn btn-primary" data-columnindex="2">LastName</a>  
                    <a class="showHide btn btn-primary" data-columnindex="3">FeesPaid</a>  
                    <a class="showHide btn btn-primary" data-columnindex="4">Gender</a>  
                    <a class="showHide btn btn-primary" data-columnindex="5">Email</a>   
                </div>  
            </div>  --%>
        <br />

        <div class="table-responsive">
            <dx:ASPxGridView ID="gvRegistration" runat="server" Width="100%" AutoGenerateColumns="False" RightToLeft="False" Theme="MetropolisBlue">
                

                <settingscommandbutton>
<ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

<HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
</settingscommandbutton>
                <columns>
                                <dx:GridViewDataColumn FieldName="Col_A" Caption="Reg Num">
<SettingsHeaderFilter>
<DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
</SettingsHeaderFilter>
                    <DataItemTemplate> 
                         <a class="gridLink" href="<%# string.Format("../Mgment/frmUpdateRegistration?RegID={0}&Name={1}", FSPBAL.Security.URLEncrypt(Eval("RegistrationID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                                <asp:Label ID="lblRegistrationNumber" runat="server" Text='<%#Eval("RegistrationID") %>'></asp:Label>
                                            </a>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="SupplierName" Caption="Supplier Name" VisibleIndex="1">                                        
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="SupplierType" Caption="Supplier Type" VisibleIndex="2">
<SettingsHeaderFilter>
<DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
</SettingsHeaderFilter>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="BusinessClassification" Caption="Business Classification" VisibleIndex="3">
<SettingsHeaderFilter>
<DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
</SettingsHeaderFilter>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="RegistrationType" Caption="Registration Type" VisibleIndex="4">
<SettingsHeaderFilter>
<DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
</SettingsHeaderFilter>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="Status" Caption="Status" VisibleIndex="5">
<SettingsHeaderFilter>
<DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
</SettingsHeaderFilter>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="CreationDateTime" Caption="Creation Date" VisibleIndex="6">
<SettingsHeaderFilter>
<DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
</SettingsHeaderFilter>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="CreatedBy" Caption="Registrated By" VisibleIndex="7">
<SettingsHeaderFilter>
<DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
</SettingsHeaderFilter>
                                    </dx:GridViewDataColumn>
                                </columns>
                <styles> 
                                     <header cssclass="gridHeader">
                                     </header>
                                     <row cssclass="gridRowOdd">
                                     </row>
                                     <alternatingrow cssclass="gridRowEven">
                                     </alternatingrow>
                                     <footer cssclass="GridFooter">
                                     </footer>
                            </styles>
            </dx:ASPxGridView>
            <%-- <dx:ASPxGridView runat="server" ID="gvRegistration" Width="100%" >
                               
                            </dx:ASPxGridView>--%>
        </div>
        <asp:SqlDataSource runat="server" ID="DsRegistration" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllRegistrationSupplier]"></asp:SqlDataSource>

        <%-- <table id="studentTable" class="table table-responsive table-hover">  
                <thead>  
                    <tr>  
                        <th  data-columnindex="0" >RegistrationID</th>  
                        <th  data-columnindex="1">SupplierName</th>  
                        <th  data-columnindex="2">BusinessClass</th>  
                        <th  data-columnindex="3" >OwnerName</th>  
                        <th  data-columnindex="4" >OfficialEmail</th>  
                        <th  data-columnindex="5" >ContactMobile</th>   
                    </tr>  
                </thead>   
            </table>  
        </div> 
        
         <script type="text/javascript">  
     $(document).ready(function () {  
         $.ajax({  
             type: "POST",  
             dataType: "json",  
             url: "RegistrationServices.asmx/LoadAllregistration",
             success: function (data) {  
                 var datatableVariable = $('#studentTable').DataTable({  
                     data: data,  
                     columns: [  
                         { 'data': 'RegistrationID' },
                         { 'data': 'SupplierName' },
                         { 'data': 'BusinessClass' },
                         { 'data': 'OwnerName' },
                         { 'data': 'OfficialEmail' },
                         { 'data': 'ContactMobile' }]
                 });  
               /*  $('#studentTable tfoot th').each(function () {  
                     var placeHolderTitle = $('#studentTable thead th').eq($(this).index()).text();  
                     $(this).html('<input type="text" class="form-control input input-sm" placeholder = "Search ' + placeHolderTitle + '" />');  
                 }); 
                 datatableVariable.columns().every(function () {  
                     var column = this;  
                     $(this.footer()).find('input').on('keyup change', function () {  
                         column.search(this.value).draw();  
                     });  
                 });   
                 $('.showHide').on('click', function () {  
                     var tableColumn = datatableVariable.column($(this).attr('data-columnindex'));  
                     tableColumn.visible(!tableColumn.visible());  
                 });  */
             }
         }); 
       });

    
 </script>--%>
    </form>
</body>
</html>
