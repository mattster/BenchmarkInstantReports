<%@ Page Title="Scan Summary" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="ScanReport.aspx.cs" Inherits="Benchmark_Instant_Reports_2.ScanReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tcol1
        {
            width: 92px;
        }
        .tcol2
        {
            width: 341px;
        }
        .tcol3
        {
            width: 150px;
        }
        .tcol4
        {
            width: 420px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Scan Summary</h1>
    <p>
        The Benchmark Scan Summary Report shows the number of unique answer sheets scanned
        in for a specified list of tests for each teacher. Use this report to help quickly
        determine if the scanning is complete for a set of tests at a campus.</p>
    <table style="width: 98%;">
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
                            BehaviorID="popupCE" PopupControlID="updpnlTestFilter" Position="Top" OffsetY="-50"
                            OffsetX="-280" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblSelectTest" CssClass="" runat="server">Select Test(s):</asp:Label>
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
                            <asp:ListBox ID="lbListTests" runat="server" Rows="10" Width="400px" SelectionMode="Multiple"
                                OnSelectedIndexChanged="lbListTests_SelectedIndexChanged"></asp:ListBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tcol1">
                &nbsp;
            </td>
            <td class="tcol2">
                &nbsp;
            </td>
            <td class="tcol3">
                &nbsp;
            </td>
            <td class="tcol4">
                Use SHIFT and CTRL keys for multiple values
            </td>
        </tr>
        <tr>
            <td class="tcol1">
                &nbsp;
            </td>
            <td class="tcol2">
                <asp:Panel ID="pnlGenReport" runat="server">
                    <asp:UpdatePanel ID="updpnlGenReport" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnGenReport" runat="server" Text="Generate Report" OnClick="btnGenReport_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <td class="tcol2">
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
                <rsweb:ReportViewer ID="repvwScanReport1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                    Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
                    <LocalReport ReportPath="ScanReportRep1.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetScanReport" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwScanReport2" runat="server" Font-Names="Verdana" Font-Size="8pt"
                    Height="700px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="500px" SizeToReportContent="True">
                    <LocalReport ReportPath="ScanReportRep2.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetScanReport" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
                    TypeName="Benchmark_Instant_Reports_2.DataSetScanReportTableAdapters.TEMP_RESULTS_SCANREPORTTableAdapter"
                    DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}"
                    UpdateMethod="Update">
                    <DeleteParameters>
                        <asp:Parameter Name="Original_CAMPUS" Type="String" />
                        <asp:Parameter Name="Original_TEST_ID" Type="String" />
                        <asp:Parameter Name="Original_TEACHER" Type="String" />
                        <asp:Parameter Name="Original_PERIOD" Type="String" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="CAMPUS" Type="String" />
                        <asp:Parameter Name="TEST_ID" Type="String" />
                        <asp:Parameter Name="TEACHER" Type="String" />
                        <asp:Parameter Name="PERIOD" Type="String" />
                        <asp:Parameter Name="NUM_SCANNED" Type="Decimal" />
                        <asp:Parameter Name="NUM_QUERIED" Type="Decimal" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="NUM_SCANNED" Type="Decimal" />
                        <asp:Parameter Name="NUM_QUERIED" Type="Decimal" />
                        <asp:Parameter Name="Original_CAMPUS" Type="String" />
                        <asp:Parameter Name="Original_TEST_ID" Type="String" />
                        <asp:Parameter Name="Original_TEACHER" Type="String" />
                        <asp:Parameter Name="Original_PERIOD" Type="String" />
                    </UpdateParameters>
                </asp:ObjectDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
