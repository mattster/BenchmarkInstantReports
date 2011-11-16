<%@ Page Title="BIR: Materials Checklist" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="MaterialsReport.aspx.cs" Inherits="Benchmark_Instant_Reports_2.Classes.WebForm5" %>

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
        #Text1
        {
            width: 363px;
        }
        #inRepHeader
        {
            width: 363px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Materials Checklist</h1>
    <p>
        Materials Checklist - Generate a Materials Checklist report for a set of tests that
        shows the number of answer sheets that meet the test criteria by teacher and period.</p>
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
                    </tr>
                    <tr>
                        <td class="style4" valign="middle" align="right">
                        </td>
                        <td align="left" class="style2" valign="middle">
                        </td>
                    </tr>
                </table>
                <br />
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
    <rsweb:ReportViewer ID="repvwMaterialsRep1" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Height="800px" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
        WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
        <LocalReport ReportPath="MaterialsChecklistRep.rdlc">
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
