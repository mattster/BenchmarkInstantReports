<%@ Page Title="BIR: Scan Summary" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ScanReport.aspx.cs" Inherits="Benchmark_Instant_Reports_2.Classes.WebForm1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 92px;
        }
        .style2
        {
            width: 341px;
        }
        .style3
        {
            width: 492px;
        }
        .style4
        {
            width: 160px;
        }
        .style5
        {
            width: 185px;
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
            <td class="style3" valign="middle">
                <table style="width: 97%;">
                    <tr>
                        <td class="style4" valign="middle" align="right">
                            Select Campus:
                        </td>
                        <td align="left" class="style2" valign="middle">
                            <asp:DropDownList ID="ddCampus" runat="server" Height="28px" Width="240px" OnSelectedIndexChanged="ddCampus_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                        <td class="style5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style4" valign="middle" align="right">
                        </td>
                        <td align="left" class="style2" valign="middle">
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="middle" class="style18">
                <table style="width: 99%; margin-left: 6px;">
                    <tr>
                        <td class="style1">
                            Select Test(s):
                        </td>
                        <td class="style15">
                            <asp:ListBox ID="lbBenchmark" runat="server" Rows="10" OnSelectedIndexChanged="lbBenchmark_SelectedIndexChanged1"
                                Width="400px" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;
                        </td>
                        <td class="style15">
                            Use SHIFT and CTRL keys for multiple values
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="style3" align="center" valign="middle">
                <asp:Button ID="btnGenReport" runat="server" Text="Generate Report" OnClick="btnGenReport_Click" />
            </td>
            <td valign="middle" class="style18">
                &nbsp;
            </td>
        </tr>
    </table>
    &nbsp;&nbsp;
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
</asp:Content>
