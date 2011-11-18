<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="Benchmark_Instant_Reports_2.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Instant Reports Login</h1>
    <p>
        Please login for a specific campus or as a District-level Administrator to continue.</p>
    <div id="loginformarea">
        <div class="row">
            &nbsp;
            <div class="formlabel">
                Select a campus</div>
            <asp:DropDownList ID="ddLoginCampus" CssClass="formDDL" runat="server" Height="28px"
                Width="240px" OnSelectedIndexChanged="ddLoginCampus_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="row">
            &nbsp;
            <div class="formlabel">
                Enter Password:</div>
            <div class="formtextbox">
                <asp:TextBox ID="tbPasswordCampus" TextMode="Password" runat="server" Width="100px" OnTextChanged="tbPasswordCampus_TextChanged" /></div>
        </div>
        <div class="row" id="loginOR">
            &nbsp;&nbsp;&nbsp;<b>-OR-</b></div>
        <div class="row">
            &nbsp;
            <div class="formlabellong">
                Login as a District Administrator</div>
        </div>
        <div class="row">
            &nbsp;
            <div class="formlabel">
                Enter Password:</div>
            <div class="formtextbox">
                <asp:TextBox ID="tbPasswordAdmin" TextMode="Password" runat="server" Width="100px"  OnTextChanged="tbPasswordAdmin_TextChanged" /></div>
        </div>
        <div class="row">
            &nbsp;
            <asp:Button ID="btnSubmit" CssClass="formbutton" OnClick="Login_Click" Text="Login" runat="server" /></div>
    </div>
    <asp:HyperLink Target="_blank" Text="" ID="modalpopupcontrol" runat="server"></asp:HyperLink>
    <asp:Panel ID="popupPanel" runat="server" CssClass="popup-dialog">
        <div id="dialogContents">
            Incorrect password. Please try again.<br />
            <asp:Button ID="ButtonOK" runat="server" Text="OK" /></div>
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="mpupIncorrectPassword" runat="server" TargetControlID="modalpopupcontrol"
        PopupControlID="popupPanel" OkControlID="ButtonOK" DropShadow="true" BackgroundCssClass="modalBackground">
      
    </ajaxToolkit:ModalPopupExtender>
</asp:Content>
