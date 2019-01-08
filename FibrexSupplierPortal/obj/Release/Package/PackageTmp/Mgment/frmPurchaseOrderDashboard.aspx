<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmPurchaseOrderDashboard.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmPurchaseOrderDashboard" %>

<%@ Register Src="~/Mgment/Control/ManagementLeftSideMenu.ascx" TagPrefix="uc1" TagName="ManagementLeftSideMenu" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/highchart/highcharts.js"></script>
    <script src="../Scripts/highchart/exporting.js"></script>
    <style>
        .panel {
            border-top-left-radius: 4px !important;
            border-top-right-radius: 4px !important;
        }
    </style>
    <script type="text/javascript">
        <%--  $(function () { 
            var getColor = {
                'Approved': '#7cb5ec',
                'Cancelled': '#434348',
                'Closed': '#f0ad4e' ,
                'Draft':'#a94442',
                'InProgress':'#4a7e3f',
                'PendingRevision': '#d9534f',
                'Revised': 'YELLOW'
            };

            $('#container').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'PO Status Distribution'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.toolbar}</b>: {point.percentage:.1f} %',
                            
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    name: 'Brands',
                    colorByPoint: true,
                    data: [{
                        name: 'Approved (' + <%=HidCountActive.Value%> + ')',
                        y: <%= decimal.Parse(HidActive.Value)%> ,
                        toolbar: 'APRV',
                        color: getColor['Active']
                    }, {
                        name: 'Cancelled (' + <%=HidCountBlackList.Value%> + ')',
                        y:  <%= decimal.Parse(HidBlackList.Value)%>,
                        color: getColor['Cancelled'],
                        toolbar: 'CANC'
                    }, {
                        name: 'Draft (' + <%=HidCountInActive.Value%> + ')',
                        y:  <%= decimal.Parse(HidInActive.Value)%>,
                        color: getColor['Draft'],
                        toolbar: 'DRFT'
                    }, {
                        name: 'In Progress (' + <%=HidCountPendingBlacklisted.Value%> + ')',
                        y:  <%= decimal.Parse(HidPendingBlacklisted.Value)%>,
                        color: getColor['INPROG'],
                        toolbar: 'INPROG'
                    },
                     {
                         name: 'Pending Revision (' + <%=HidCountPendingActive.Value%> + ')',
                         y:  <%= decimal.Parse(HidPendingActive.Value)%>,
                         color: getColor['PNDREV'],
                         toolbar: 'PNDREV'
                     },
                     {
                         name: 'Revised (' + <%=HidCountWarning.Value%> + ')',
                         y:  <%= decimal.Parse(HidPendingWarning.Value)%>,
                         color: getColor['Revised'],
                         toolbar: 'REVISD'
                     }]
                }] 
            },function(chart){
                var minValue = 0.001
        
                $.each(chart.series[0].data, function(i, point){
                    if(point.y < minValue) {
                        point.setVisible(false);
                    }
                });
            });
        });--%>
        $(function () {
            $('#PODistributionOrganization').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: ' Approved PO Distribution by Division'
                },
                xAxis: {
                    categories: [<%=HidOrgName.Value%>] 
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                series: [{ 
                    data: [<%=HidOrgData.Value%>]
                }]
            });
        });
        $(function () {
            $('#POStatusDistribution').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'PO Status Distribution'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.toolbar}</b>: {point.y}',

                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    name: 'Brands',
                    colorByPoint: true,
                    data: [ <%=HidPoStatusData.Value%>]
                }]
            }
            , function (chart) {
                //ar minValue = 0.001

                //$.each(chart.series[0].data, function(i, point){
                //    if(point.y < minValue) {
                //        point.setVisible(false);
                //    }
                //});
            });
        });
        $(function () {
            $('#POStatusProjectWise').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'PO Distribution by Project'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.toolbar}</b>: {point.y}',

                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    name: 'Brands',
                    colorByPoint: true,
                    data: [ <%=HidProjectData.Value%>]
                }]
            }
            , function (chart) {
                //ar minValue = 0.001

                //$.each(chart.series[0].data, function(i, point){
                //    if(point.y < minValue) {
                //        point.setVisible(false);
                //    }
                //});
            });
        });

        $(function () {
            $('#PODistributionbyBuyer').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'PO Distribution by Buyer'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.toolbar}</b>: {point.y}',

                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    name: 'Brands',
                    colorByPoint: true,
                    data: [ <%=HidBuyerData.Value%>]
                }]
            }
            , function (chart) {
                //ar minValue = 0.001

                //$.each(chart.series[0].data, function(i, point){
                //    if(point.y < minValue) {
                //        point.setVisible(false);
                //    }
                //});
            });
        });
        $(function () {
            $('#ContractsDistributionbyType').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Contracts Distribution by Type'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.toolbar}</b>: {point.y}',

                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    name: 'Brands',
                    colorByPoint: true,
                    data: [ <%=HIdContractDescription.Value%>]
                }]
            }
            , function (chart) {
                //ar minValue = 0.001

                //$.each(chart.series[0].data, function(i, point){
                //    if(point.y < minValue) {
                //        point.setVisible(false);
                //    }
                //});
            });
        });
        //
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:ManagementLeftSideMenu runat="server" ID="DashboardLeftSideMenu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="RPTheadingName">
        Purchase Order Dashboard
          <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px;">
              &nbsp;              
          </div>
    </div>
    <div class=".page-header">&nbsp;</div>
    <div class="row">
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-users fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblApprovedPurchaseOrder" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>
                                Approved Purchase Order<br /><br />
                            </div>
                        </div>
                    </div>
                </div>
                <a href="frmSearchPOList?Status=APRV&ID=E2PxROEgkuNLqa7Kplp/Hg==&revision=RE6F8TWA9wpCtVKLK/kIEg==">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-yellow">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-gears fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblPendingRevisionPurchaseOrder" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>
                                Purchase Orders Pending<br />Revision
                            </div>
                        </div>
                    </div>
                </div>
                <a href="frmSearchPOList?Status=PNDREV&ID=E2PxROEgkuNLqa7Kplp/Hg==&revision=RE6F8TWA9wpCtVKLK/kIEg==">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-green">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-tasks fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblTotalContract" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>
                                Contracts <br /><br />
                            </div>
                        </div>
                    </div>
                </div>
                <a href="frmSearchPOContract">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
    </div>
    <div style="clear: both;">&nbsp;</div>
    <div class=".page-header">&nbsp;</div>
    <div class="col-lg-12">
        <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        </div>
        <div class="col-sm-6" style="border-right: 1px solid #e7e7e7;">
            <div id="POStatusDistribution" style="min-width: 410px; height: 400px; margin: 0 auto"></div>
        </div>
        <div class="col-sm-6">
            <div id="PODistributionOrganization" style="min-width: 410px; height: 400px; margin: 0 auto"></div>
        </div>
    </div>
    <br />
    <br />
    <div style="clear: both;">&nbsp;</div>
    <div class="col-lg-12" style="margin-top: 50px;">
        <div class="col-sm-6" style="border-right: 1px solid #e7e7e7;">
            <div id="POStatusProjectWise" style="min-width: 410px; height: 400px; margin: 0 auto"></div>
        </div>
        <div class="col-sm-6">
            <div id="PODistributionbyBuyer" style="min-width: 410px; height: 400px; margin: 0 auto"></div>
        </div>
    </div>
    <div class="col-lg-12" style="margin-top: 50px;">
        <div class="col-sm-6" style="border-right: 1px solid #e7e7e7;">
            <div id="ContractsDistributionbyType" style="min-width: 410px; height: 400px; margin: 0 auto"></div>
        </div>
        <div class="col-sm-6"> 
        </div>
    </div>
    <div style="clear: both;">&nbsp;</div>
    <div class=".page-header">&nbsp;</div>


    <asp:HiddenField ID="HidOrgData" runat="server" />
    <asp:HiddenField ID="HidOrgName" runat="server" />
    <asp:HiddenField ID="HidPoStatusData" runat="server" />
    <asp:HiddenField ID="HidProjectData" runat="server" />
    <asp:HiddenField ID="HidBuyerData" runat="server" />
    <asp:HiddenField ID="HIdContractDescription" runat="server" />

</asp:Content>
