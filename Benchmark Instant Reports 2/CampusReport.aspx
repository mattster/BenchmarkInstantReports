<%@ Page Title="BIR: Campus Summary" Language="C#" MasterPageFile="~/Site.Master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CampusReport.aspx.cs"
    Inherits="Benchmark_Instant_Reports_2.CampusReport" %>

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
            width: 160px;
            text-align: right;
        }
        .tcol4
        {
            width: 230px;
        }
        .tcolCustom1
        {
            width: 492px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Campus Summary</h1>
    <p>
        The Campus Report shows passing data for a specific campus for a specific Test.</p>
    <table>
        <tr>
            <td class="tcol1">
                <br />
                Select Campus:
            </td>
            <td class="tcol2">
                <br />
                <asp:Panel ID="pnlCampus" runat="server">
                    <asp:UpdatePanel ID="updpnlCampus" class="CampusUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddCampus" runat="server" Height="28px" Width="240px" OnSelectedIndexChanged="ddCampus_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <td class="tcol3">
                <asp:Panel ID="pnlTestFilter" CssClass="FilterPanel" runat="server">
                    <asp:UpdatePanel ID="updpnlTestFilter" class="FilterPanel" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td class="FilterHdrCol1">
                                        <div class="popupH1">
                                            Filter Tests By</div>
                                    </td>
                                    <td class="FilterHdrCol2">
                                        <asp:ImageButton ID="CloseTestFilterImg" runat="server" CssClass="PopupCloseImg, floatright"
                                            OnClientClick="hidePCE()" AlternateText="Close Popup" ImageUrl="~/content/images/icon_close_window.gif" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FilterHdrCol1">
                                        <span style="font-weight: bold;">Test List is instantly updated. Click outside popup
                                            to continue.</span>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td class="FilterCol1">
                                        <div class="popupLabel">
                                            Curriculum Area:</div>
                                    </td>
                                    <td class="FilterCol2">
                                        <asp:DropDownList ID="ddTFCur" CssClass="popupDDL" runat="server" Height="28px" Width="150px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddTFCur_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FilterCol1">
                                        <div class="popupLabel">
                                            Subject:</div>
                                    </td>
                                    <td class="FilterCol2">
                                        <asp:DropDownList ID="ddTFSubj" CssClass="popupDDL" runat="server" Height="28px"
                                            Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddTFSubj_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FilterCol1">
                                        <div class="popupLabel">
                                            Test Type:</div>
                                    </td>
                                    <td class="FilterCol2">
                                        <asp:DropDownList ID="ddTFTestType" CssClass="popupDDL" runat="server" Height="28px"
                                            Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddTFTestType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FilterCol1">
                                        <div class="popupLabel">
                                            Test Version:</div>
                                    </td>
                                    <td class="FilterCol2">
                                        <asp:DropDownList ID="ddTFTestVersion" CssClass="popupDDL" runat="server" Height="28px"
                                            Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddTFTestVersion_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FilterCol1">
                                        &nbsp;<br />
                                        &nbsp;
                                    </td>
                                    <td class="FilterCol2">
                                        <asp:Button ID="btnTFReset" CssClass="popupButton" runat="server" Text="Reset" OnClick="btnTFReset_Click"
                                            OnClientClick="hidePCE()" UseSubmitBehavior="false" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <br />
                <asp:UpdatePanel ID="updpnlImgFilterTests" runat="server">
                    <ContentTemplate>
                        <asp:Image CssClass="filterImg, floatleft" ID="imgFilterTests" runat="server" AlternateText="Filter Tests"
                            ImageUrl="~/content/images/f-circ-20x20.png" />
                        <ajaxToolkit:PopupControlExtender ID="pceFilterTests" TargetControlID="imgFilterTests"
                            BehaviorID="popupCE" PopupControlID="updpnlTestFilter" Position="Top" OffsetY="-20"
                            OffsetX="-280" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblSelectTest" CssClass="" runat="server">Select Test:</asp:Label>
            </td>
            <td class="tcol4">
                <asp:Panel ID="pnlBenchmark" runat="server">
                    <asp:UpdatePanel ID="updpnlBenchmark" class="BenchmarkUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="updpnlFilteredTestsLabel" class="DDLabelAbove" runat="server">
                                <ContentTemplate>
                                    &nbsp;
                                    <asp:Label ID="lblTestsFiltered" CssClass="DDLabelAboveText" Visible="false" runat="server">Test List is Filtered. Click Filter Button to change.</asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:DropDownList ID="listTests" runat="server" Height="28px" Width="300px" OnSelectedIndexChanged="listTests_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tcol1">
                <br />
                &nbsp;
            </td>
            <td class="tcol2">
                <br />
                &nbsp;
            </td>
            <td class="tcol3">
                <br />
                &nbsp;
            </td>
            <td class="tcol4">
                <br />
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="tcol1">
            </td>
            <td class="tcol2">
                <asp:Panel ID="pnlGenReport" runat="server">
                    <asp:UpdatePanel ID="updpnlGenReport" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnGenReport" runat="server" Text="Generate Report" OnClick="btnGenReport_Click"
                                OnClientClick="" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <td class="tcol4">
                <asp:UpdateProgress ID="progressGenReport" runat="server" AssociatedUpdatePanelID="updpnlGenReport">
                    <ProgressTemplate>
                        <asp:Image ID="imgReportProgress" runat="server" CssClass="imgLoader, floatleft"
                            AlternateText="loading..." ImageUrl="~/content/images/ajax-loader1.gif" />
                        <div class="popupH1">
                            Generating Report ...</div>
                        <asp:Label ID="lblReportProgress" runat="server"></asp:Label></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
    </table>
    &nbsp;&nbsp;
    <asp:Panel ID="pnlReportViewer" runat="server">
        <asp:UpdatePanel ID="updpnlReportViewer" runat="server">
            <ContentTemplate>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
