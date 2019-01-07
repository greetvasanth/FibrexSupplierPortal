<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="recaptach.ascx.cs" Inherits="FibrexSupplierPortal.Mgment.Control.recaptach" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<div class="reg-panel panel panel-default">
                    <div class="panel-body">
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                   <asp:UpdatePanel ID="upCaptach" runat="server">
                                            <ContentTemplate>

                                <b>Security Check Required</b><br />
                                Before submitting, please type the security code below.<br />
                                <br />
                                <div class="form-group">
                                    <div class="col-sm-5">
                                        <%-- <img src="Handler1.ashx" />--%>
                                        <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                                            CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                            FontColor="#0010b4" NoiseColor="#B1B1B1" />
                                    </div>
                                    <div class="col-md-1">
                                        <div style="margin-top: 50%;">
                                            <asp:ImageButton ImageUrl="~/images/refresh.png" ID="imgRefersh" runat="server" CausesValidation="false" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group" style="margin-left: 1px;">
                                    <label class="control-label Pdringtop" for="inputName">Enter Text</label>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control" MaxLength="5"></asp:TextBox>
                                    </div>
                                </div>

                                 </ContentTemplate>
                                <%-- <Triggers>
                                        <asp:PostBackTrigger ControlID="imgRefersh" />
                                    </Triggers>--%>
                                     </asp:UpdatePanel>
                            </div>
                        
                        </div>
                    </div>
                </div>