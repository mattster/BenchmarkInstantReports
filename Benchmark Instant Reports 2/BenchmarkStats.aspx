<%@ Page Title="Item Analysis" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="BenchmarkStats.aspx.cs" Inherits="Benchmark_Instant_Reports_2.BenchmarkStats" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .tcol1
        {
            width: 110px;
            text-align: right;
            margin-right: 5px;
        }
        .tcol2
        {
            width: 410px;
        }
        .tcol3
        {
            width: 100px; /*text-align: right;*/
            margin-right: 5px;
        }
        .tcol4
        {
            width: 230px;
        }
        .tcolCustom1
        {
            width: 500px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Item Analysis</h1>
    <p>
        The Item Analysis Reports show question-by-question performance for a specific test
        and group of teachers on a campus.</p>
    <table>
        <tr>
            <td class="tcol1">
                Select Campus:
            </td>
            <td class="tcol2">
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
                Select Report Type:
            </td>
            <td class="tcol4">
                <asp:Panel ID="pnlReportType" runat="server">
                    <asp:UpdatePanel ID="updpnlReportType" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddRepType" runat="server" Height="28px" Width="220px" OnSelectedIndexChanged="ddRepType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tcol1">
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
                            OffsetX="550" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblSelectTest" CssClass="" runat="server">Select Test:</asp:Label>
                <br />
                &nbsp;<br />
                &nbsp;
            </td>
            <td class="tcol2">
                <asp:Panel ID="pnlBenchmark" runat="server">
                    <asp:UpdatePanel ID="updpnlBenchmark" class="BenchmarkUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="updpnlFilteredTestsLabel" class="DDLabelAbove" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="listTests" runat="server" Height="28px" Width="400px" OnSelectedIndexChanged="listTests_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <br />
                                    &nbsp;
                                    <asp:Label ID="lblTestsFiltered" CssClass="DDLabelAboveText" Visible="false" runat="server">Test List is Filtered. Click Filter Button to change.</asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <td class="tcol3">
                <asp:Panel ID="pnlLblTeacher" runat="server">
                    <asp:UpdatePanel ID="updpnlLblTeacher" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblSelectTeacher" runat="server">Select Teacher:</asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <td class="tcol4">
                <asp:Panel ID="pnlTeacher" runat="server">
                    <asp:UpdatePanel ID="updpnlTeacher" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddTeacher" runat="server" Height="28px" Width="220px" OnSelectedIndexChanged="ddTeacher_SelectedIndexChanged">
                            </asp:DropDownList>
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
                <asp:Panel ID="pnlNoScanData" runat="server">
                    <asp:UpdatePanel ID="updpnlNoScanData" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblNoScanData" runat="server" ForeColor="#0000FF">There is no scanned data currently available for this test at this campus</asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <td class="tcol3">
                Group By:
            </td>
            <td class="tcol4">
                <asp:Panel ID="pnlGroupBy" runat="server">
                    <asp:UpdatePanel ID="updpnlGroupBy" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddGroupBy" runat="server" Height="28px" Width="220px" OnSelectedIndexChanged="ddGroupBy_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tcol1">
            </td>
            <td class="tcol1">
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
    <br />
    <asp:Panel ID="pnlReportViewer" runat="server">
        <asp:UpdatePanel ID="updpnlReportViewer" runat="server">
            <ContentTemplate>
                <rsweb:ReportViewer ID="repvwBenchmarkStats1a" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" Height="800px" SizeToReportContent="True">
                    <LocalReport ReportPath="BenchmarkStatsRep1a.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwBenchmarkStats1b" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
                    <LocalReport ReportPath="BenchmarkStatsRep1b.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwBenchmarkStats1c" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" Height="800px" InteractiveDeviceInfos="(Collection)" SizeToReportContent="True"
                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="700px">
                    <LocalReport ReportPath="BenchmarkStatsRep1c.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwBenchmarkStats2a" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
                    <LocalReport ReportPath="BenchmarkStatsRep2a.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwBenchmarkStats2b" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
                    <LocalReport ReportPath="BenchmarkStatsRep2b.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwBenchmarkStats2c" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
                    <LocalReport ReportPath="BenchmarkStatsRep2c.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwBenchmarkStats3a" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
                    <LocalReport ReportPath="BenchmarkStatsRep3a.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwBenchmarkStats3b" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
                    <LocalReport ReportPath="BenchmarkStatsRep3b.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwBenchmarkStats3c" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
                    <LocalReport ReportPath="BenchmarkStatsRep3c.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwBenchmarkStats4a" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
                    <LocalReport ReportPath="BenchmarkStatsRep4a.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwBenchmarkStats4b" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
                    <LocalReport ReportPath="BenchmarkStatsRep4b.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="repvwBenchmarkStats4c" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
                    <LocalReport ReportPath="BenchmarkStatsRep4c.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetBenchmarkStats" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
                    TypeName="Benchmark Instant Reports 2.DataSetTableAdapters.TEMP_RESULTS_BENCHMARKSTATSTableAdapter">
                </asp:ObjectDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
