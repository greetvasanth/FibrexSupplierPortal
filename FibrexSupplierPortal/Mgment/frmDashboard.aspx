<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmDashboard.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmDashboard" %>

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
        $(function () { 
            var getColor = {
                'Active': '#7cb5ec',
                'Blacklist': '#434348',
                'Inactive': '#f0ad4e' ,
                'PBLKT':'#a94442',
                'PACT':'#4a7e3f',
                'WARNG': '#d9534f' 

            };

            $('#container').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Supplier Status Distribution'
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
                        name: 'Active (' + <%=HidCountActive.Value%> + ')',
                        y: <%= decimal.Parse(HidActive.Value)%> ,
                        toolbar: 'ACT',
                        color: getColor['Active']
                    }, {
                        name: 'Blacklisted (' + <%=HidCountBlackList.Value%> + ')',
                        y:  <%= decimal.Parse(HidBlackList.Value)%>,
                        color: getColor['Blacklist'],
                        toolbar: 'BLKT'
                    }, {
                        name: 'Profile Requires Update (' + <%=HidCountInActive.Value%> + ')',
                        y:  <%= decimal.Parse(HidInActive.Value)%>,
                        color: getColor['Inactive'],
                        toolbar: 'UPRQD'
                    }, {
                        name: 'Blacklisted/pending Approval (' + <%=HidCountPendingBlacklisted.Value%> + ')',
                        y:  <%= decimal.Parse(HidPendingBlacklisted.Value)%>,
                        color: getColor['PBLKT'],
                        toolbar: 'PBLKT'
                    },
                     {
                         name: 'Reactivated/pending Approval (' + <%=HidCountPendingActive.Value%> + ')',
                         y:  <%= decimal.Parse(HidPendingActive.Value)%>,
                         color: getColor['PACT'],
                         toolbar: 'PACT'
                     },
                     {
                         name: 'Warning (' + <%=HidCountWarning.Value%> + ')',
                         y:  <%= decimal.Parse(HidPendingWarning.Value)%>,
                         color: getColor['WARNG'],
                         toolbar: 'WARNG'
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
        });
    </script>
    <script type="text/javascript">
        
        $(function () {   var getColor = {
            'New': '#337ab7',
            'APRV': '#4a7e3f',
            'PAPR': 'Yellow' ,
            'REOP':'#8085e9',
            'STPD':'#f0ad4e',
            'REJD': '#d9534f',
            'CANC':'#434348'
        };

            $('#RegistrationStatusContainer').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Registration Status Distribution'
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
                        name:'New/Pending Approval (' + <%=HidCountNew.Value%> + ')',
                        y: <%= decimal.Parse(HidNewPendingApproval.Value)%>,
                        toolbar:'NEW',
                        color: getColor['New']
                    },{
                        name: 'Approved (' + <%=HidCountAprv.Value%> + ')',
                            y:  <%= decimal.Parse(HidApproved.Value)%>,
                            toolbar:'APRV',
                            color: getColor['APRV'] 
                        }, {
                            name: 'Revised/Pending Approval (' + <%=HidCountPAPR.Value%> + ')',
                                y:   <%= decimal.Parse(HidPendingApproval.Value)%>,                              
                                selected: true,  toolbar:'PAPR',
                                color: getColor['PAPR']
                            },  {
                                name: 'Reopened/Pending Approval (' + <%=HidCountREOP.Value%> + ')',
                                y:  <%= decimal.Parse(HidReopened.Value)%>,
                                color: getColor['REOP'],
                                toolbar:'REOP'
                            },{
                                name: 'Supplier to Provide Details (' + <%=HidCountSTPD.Value%> + ')',
                                    y:  <%= decimal.Parse(HidSTPD.Value)%>,
                                    color: getColor['STPD'],
                                    toolbar:'STPD'
                                },{
                                    name: 'Rejected (' + <%=HidCountRej.Value%> + ')',
                                        y:  <%= decimal.Parse(HidRejected.Value)%>,
                                        color: getColor['REJD'],
                                        toolbar:'REJD'
                                    },{
                                        name: 'Cancelled (' + <%=HidCountCanc.Value%> + ')',
                                            y: <%= decimal.Parse(HidCancel.Value)%>,
                                            color: getColor['CANC'],
                                            toolbar:'CANC'
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
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:ManagementLeftSideMenu runat="server" ID="ManagementLeftSideMenu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="RPTheadingName">
       Supplier Dashboard
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
                                <asp:Label ID="lblTotalRegistration" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>
                                New Registrations<br />
                                &nbsp;
                            </div>
                        </div>
                    </div>
                </div>
                <a href="frmSearchRegistration?RegType=External&Status=Pending">
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
                                <asp:Label ID="lblSTPDendingRegistrations" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>
                                Pending Supplier to<br />
                                Provide Details 
                            </div>
                        </div>
                    </div>
                </div>
                <a href="frmSearchRegistration?RegType=External&Status=SupplierToProvideDetail">
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
                                <asp:Label ID="lblPendingProfileChangeRequest" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>
                                Pending Profile<br />
                                Change Requests
                            </div>
                        </div>
                    </div>
                </div>
                <a href="frmSearchProfileChangeRequest?Status=Pending">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-red">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-warning fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblSupplierExpireTradeLicense" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>
                                Expired Trade<br />
                                Licenses
                            </div>
                        </div>
                    </div>
                </div>
                <a href="frmSearchSupplier?Status=ExpireDate">
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
            <div id="container" style="min-width: 410px; height: 400px; margin: 0 auto"></div>
        </div>
        <div class="col-sm-6">
            <div id="RegistrationStatusContainer" style="min-width: 410px; height: 400px; margin: 0 auto"></div>
        </div>
    </div>

    <div style="clear: both;">&nbsp;</div>
    <div class=".page-header">&nbsp;</div>

    <asp:HiddenField ID="HidActive" runat="server" />
    <asp:HiddenField ID="HidInActive" runat="server" />
    <asp:HiddenField ID="HidBlackList" runat="server" />
    <asp:HiddenField ID="HidPendingBlacklisted" runat="server" />
    <asp:HiddenField ID="HidPendingActive" runat="server" />
    <asp:HiddenField ID="HidPendingWarning" runat="server" />

    <asp:HiddenField ID="HidCountActive" runat="server" />
    <asp:HiddenField ID="HidCountInActive" runat="server" />
    <asp:HiddenField ID="HidCountBlackList" runat="server" />
    <asp:HiddenField ID="HidCountPendingBlacklisted" runat="server" />
    <asp:HiddenField ID="HidCountPendingActive" runat="server" />
    <asp:HiddenField ID="HidCountWarning" runat="server" />

    <asp:HiddenField ID="HidApproved" runat="server" />
    <asp:HiddenField ID="HidRejected" runat="server" />
    <asp:HiddenField ID="HidReopened" runat="server" />
    <asp:HiddenField ID="HidSTPD" runat="server" />
    <asp:HiddenField ID="HidNewPendingApproval" runat="server" />
    <asp:HiddenField ID="HidPendingApproval" runat="server" />
    <asp:HiddenField ID="HidCancel" runat="server" />

    <asp:HiddenField ID="HidNewHeading" runat="server" />
    <asp:HiddenField ID="HidAprvHeading" runat="server" />
    <asp:HiddenField ID="HidRejHeading" runat="server" />
    <asp:HiddenField ID="HidSTPDHeading" runat="server" />
    <asp:HiddenField ID="HidPAPRheading" runat="server" />
    <asp:HiddenField ID="HidCancHeading" runat="server" />
    <asp:HiddenField ID="HidReopHeading" runat="server" />

    <asp:HiddenField ID="HidCountNew" runat="server" />
    <asp:HiddenField ID="HidCountAprv" runat="server" />
    <asp:HiddenField ID="HidCountRej" runat="server" />
    <asp:HiddenField ID="HidCountSTPD" runat="server" />
    <asp:HiddenField ID="HidCountPAPR" runat="server" />
    <asp:HiddenField ID="HidCountCanc" runat="server" />
    <asp:HiddenField ID="HidCountREOP" runat="server" />

</asp:Content>
