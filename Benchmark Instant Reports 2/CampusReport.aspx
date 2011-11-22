﻿<%@ Page Title="BIR: Campus Summary" Language="C#" MasterPageFile="~/Site.Master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CampusReport.aspx.cs"
    Inherits="Benchmark_Instant_Reports_2.Classes.WebForm8" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tcol1
        {
            width: 160px;
        }
        .tcol2
        {
            width: 300px;
        }
        .tcol3
        {
            width: 190px;
        }
        .tcolCustom1
        {
            width: 492px;
        }
        .style5
        {
            width: 185px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Campus Summary</h1>
    <p>
        The Campus Report shows passing data for a specific campus for a specific Test.</p>
    <table style="width: 98%;">
        <tr>
            <td class="tcol1" valign="middle" align="right">
                <br />
                Select Campus:
            </td>
            <td align="left" class="tcol2" valign="middle">
                <br />
                <asp:Panel ID="pnlCampus" runat="server">
                    <asp:UpdatePanel ID="updpnlCampus" class="CampusUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddCampus" runat="server" Height="28px" Width="240px" OnSelectedIndexChanged="ddCampus_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <td class="tcol3">
                <asp:Panel ID="pnlTestFilter" CssClass="FilterPanel" runat="server">
                    <asp:UpdatePanel ID="updpnlTestFilter" class="FilterPanel" runat="server">
                        <ContentTemplate>
                            <div class="popupH1">
                                Filter Tests By</div>
                            <div class="popupLabel">
                                Curriculum Area:</div>
                            <asp:DropDownList ID="ddTFCur" CssClass="popupDDL" runat="server" Height="28px" Width="150px"
                                AutoPostBack="true" OnSelectedIndexChanged="popupDDLCur_SelectedIndexChanged2">
                            </asp:DropDownList>
                            <div class="popupLabel">
                                Test Type:</div>
                            <asp:DropDownList ID="ddTFTestType" CssClass="popupDDL" runat="server" Height="28px"
                                Width="150px" AutoPostBack="true" OnSelectedIndexChanged="popupDDLTestType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <br />
                <asp:UpdatePanel ID="updpnlImgFilterTests" runat="server">
                    <ContentTemplate>
                        <asp:Image CssClass="filterImg" ID="imgFilterTests" runat="server" AlternateText="Filter Tests"
                            ImageUrl="~/content/images/f-circ-20x20.png" />
                        <ajaxToolkit:PopupControlExtender ID="pceFilterTests" TargetControlID="imgFilterTests"
                            PopupControlID="updpnlTestFilter" Position="Top" OffsetY="-20" OffsetX="-280"
                            runat="server" CommitScript="popupDDLCur_SelectedIndexChanged2" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblSelectTest" CssClass="" runat="server">Select Test:</asp:Label>
            </td>
            <td>
                <asp:Panel ID="pnlBenchmark" runat="server">
                    <asp:UpdatePanel ID="updpnlBenchmark" class="BenchmarkUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="updpnlFilteredTestsLabel" class="DDLabelAbove" runat="server">
                                <ContentTemplate>
                                    &nbsp;
                                    <asp:Label ID="lblTestsFiltered" CssClass="DDLabelAboveText" Visible="false" runat="server">Test List is Filtered. Click Filter Button to change.</asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:DropDownList ID="ddBenchmark" runat="server" Height="28px" Width="300px" OnSelectedIndexChanged="ddBenchmark_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tcol1" valign="middle" align="right">
            </td>
            <td align="left" class="tcol2" valign="middle">
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="tcolCustom1" align="center" valign="middle">
                <asp:Button ID="btnGenReport" runat="server" Text="Generate Report" OnClick="btnGenReport_Click" />
            </td>
        </tr>
    </table>
    </ajaxToolkit:ModalPopupExtender>
    <%--ajaxToolkit:CascadingDropDown ID="ccddCampus" runat="server" ServicePath="CascadingDropDown1.asmx"
        ServiceMethod="GetCampusList" TargetControlID="ddCampus" Category="Campus" PromptText="--Select Campus--" />--%>
    <%-- <ajaxToolkit:CascadingDropDown ID="ccddTest" runat="server" ServicePath="CascadingDropDown1.asmx"
        ServiceMethod="GetTestsForCampus" TargetControlID="ddBenchmark" Category="Test"
        PromptText="--Select Test--" ParentControlID="ddCampus" LoadingText="Loading Tests..." />--%>
    &nbsp;&nbsp;
    <rsweb:ReportViewer ID="repvwCampusReport1" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Height="100px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
        WaitMessageFont-Size="14pt" Width="550px" SizeToReportContent="True">
        <LocalReport ReportPath="CampusReport1.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetStudentStatsC" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwCampusReport2" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Height="100px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
        WaitMessageFont-Size="14pt" Width="550px" SizeToReportContent="True">
        <LocalReport ReportPath="CampusReport2.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetStudentStatsC" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
        TypeName="Benchmark_Instant_Reports_2.DataSetStudentStatsTableAdapters.TEMP_RESULTS_STUDENTSTATSTableAdapter"
        DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}"
        UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="Original_STUDENT_ID" Type="String" />
            <asp:Parameter Name="Original_TEST_ID" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="STUDENT_ID" Type="String" />
            <asp:Parameter Name="STUDENT_NAME" Type="String" />
            <asp:Parameter Name="TEST_ID" Type="String" />
            <asp:Parameter Name="SCAN_DATETIME" Type="DateTime" />
            <asp:Parameter Name="LETTER_GRADE" Type="String" />
            <asp:Parameter Name="NUM_CORRECT" Type="Decimal" />
            <asp:Parameter Name="NUM_TOTAL" Type="Decimal" />
            <asp:Parameter Name="PCT_CORRECT" Type="Decimal" />
            <asp:Parameter Name="CAMPUS" Type="String" />
            <asp:Parameter Name="TEACHER" Type="String" />
            <asp:Parameter Name="PERIOD" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="STUDENT_NAME" Type="String" />
            <asp:Parameter Name="SCAN_DATETIME" Type="DateTime" />
            <asp:Parameter Name="LETTER_GRADE" Type="String" />
            <asp:Parameter Name="NUM_CORRECT" Type="Decimal" />
            <asp:Parameter Name="NUM_TOTAL" Type="Decimal" />
            <asp:Parameter Name="PCT_CORRECT" Type="Decimal" />
            <asp:Parameter Name="CAMPUS" Type="String" />
            <asp:Parameter Name="TEACHER" Type="String" />
            <asp:Parameter Name="PERIOD" Type="String" />
            <asp:Parameter Name="Original_STUDENT_ID" Type="String" />
            <asp:Parameter Name="Original_TEST_ID" Type="String" />
        </UpdateParameters>
    </asp:ObjectDataSource>
</asp:Content>
