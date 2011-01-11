﻿<%@ Page Title="BIR: Item Analysis Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BenchmarkStats.aspx.cs" Inherits="Benchmark_Instant_Reports_2.WebForm3" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 155px;
        }
        .style2
        {
            width: 216px;
        }
        .style3
        {
            width: 188px;
        }
        .style4
        {
            width: 160px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        The 
        Item Analysis Reports show question-by-question performance for a 
        specific test and group of teachers on a campus.</p>
    <p>
        <table style="width: 86%;">
            <tr>
                <td align="right" class="style1">
                    Select Campus:</td>
                <td class="style2">
                    <asp:DropDownList ID="ddCampus" runat="server" Height="28px" Width="240px" 
                        onselectedindexchanged="ddCampus_SelectedIndexChanged1">
                    </asp:DropDownList>
                </td>
                <td class="style4" align="right">
                    Select Report Type:</td>
                <td class="style3">
                    <asp:DropDownList ID="ddRepType" runat="server" Height="28px" 
                        onselectedindexchanged="ddRepType_SelectedIndexChanged1" Width="220px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" class="style1">
                    <strong __designer:mapid="189">
    <asp:Label ID="lblEnterSchoolPassword" runat="server" Font-Bold="True" 
        ForeColor="#3366FF" Text="Enter school password:"></asp:Label>
    </strong>
                </td>
                <td class="style2">
                    <strong __designer:mapid="189">
                <asp:TextBox ID="txtbxSchoolPassword" runat="server" Width="68px" 
                        TextMode="Password"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnEnterPassword" runat="server" 
        onclick="btnEnterPassword_Click" Text="Submit" />
    </strong>
                </td>
                <td class="style4">
                    <strong __designer:mapid="189">
    <asp:Label ID="lblIncorrectPassword" runat="server" Font-Bold="False" 
        ForeColor="#FF3300" Text="Incorrect school password."></asp:Label>
    </strong>
                </td>
                <td class="style3">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="right" class="style1">
                    Select Benchmark:</td>
                <td class="style2">
                    <asp:DropDownList ID="ddBenchmark" runat="server" Height="28px" Width="240px" 
                        onselectedindexchanged="ddBenchmark_SelectedIndexChanged1">
                    </asp:DropDownList>
                </td>
                <td class="style4" align="right">
                    <asp:Label ID="lblSelectTeacher" runat="server" Text="Select Teacher:"></asp:Label>
                </td>
                <td class="style3">
                    <asp:DropDownList ID="ddTeacher" runat="server" Height="28px" Width="220px" 
                        onselectedindexchanged="ddTeacher_SelectedIndexChanged1">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td class="style2">
                    &nbsp;</td>
                <td class="style4" align="right">
                    <asp:Label ID="lblGroupBy" runat="server" Text="Group By:"></asp:Label>
                </td>
                <td class="style3">
                    <asp:DropDownList ID="ddGroupBy" runat="server" Height="28px" 
                        onselectedindexchanged="ddGroupBy_SelectedIndexChanged" Width="220px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td align="center" class="style2">
                    <strong __designer:mapid="189">
                        <asp:Button ID="btnGenReport" runat="server" Text="Generate Report" 
    onclick="btnGenReport_Click" />
    </strong>
                </td>
                <td class="style4">
                    &nbsp;</td>
                <td class="style3">
                    &nbsp;</td>
            </tr>
        </table>
    </p>
<br />
<rsweb:ReportViewer ID="repvwBenchmarkStats1a" runat="server" 
    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="700px" 
    Height="800px" SizeToReportContent="True">
    <LocalReport ReportPath="BenchmarkStatsRep1a.rdlc">
        <DataSources>
            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                Name="DataSetBenchmarkStats" />
        </DataSources>
    </LocalReport>
</rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwBenchmarkStats1b" runat="server" 
    Font-Names="Verdana" Font-Size="8pt" Height="800px" 
    InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
    WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
        <LocalReport ReportPath="BenchmarkStatsRep1b.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetBenchmarkStats" />
            </DataSources>
        </LocalReport>
</rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwBenchmarkStats1c" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" Height="800px" 
        InteractiveDeviceInfos="(Collection)" SizeToReportContent="True" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="700px">
        <LocalReport ReportPath="BenchmarkStatsRep1c.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetBenchmarkStats" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwBenchmarkStats2a" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" Height="800px" 
        InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
        WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
        <LocalReport ReportPath="BenchmarkStatsRep2a.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetBenchmarkStats" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwBenchmarkStats2b" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" Height="800px" 
        InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
        WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
        <LocalReport ReportPath="BenchmarkStatsRep2b.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetBenchmarkStats" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwBenchmarkStats2c" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" Height="800px" 
        InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
        WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
        <LocalReport ReportPath="BenchmarkStatsRep2c.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetBenchmarkStats" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwBenchmarkStats3a" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" Height="800px" 
        InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
        WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
        <LocalReport ReportPath="BenchmarkStatsRep3a.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetBenchmarkStats" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwBenchmarkStats3b" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" Height="800px" 
        InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
        WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
        <LocalReport ReportPath="BenchmarkStatsRep3b.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetBenchmarkStats" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwBenchmarkStats3c" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" Height="800px" 
        InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
        WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
        <LocalReport ReportPath="BenchmarkStatsRep3c.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetBenchmarkStats" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwBenchmarkStats4a" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" Height="800px" 
        InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
        WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
        <LocalReport ReportPath="BenchmarkStatsRep4a.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetBenchmarkStats" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwBenchmarkStats4b" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" Height="800px" 
        InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
        WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
        <LocalReport ReportPath="BenchmarkStatsRep4b.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetBenchmarkStats" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="repvwBenchmarkStats4c" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" Height="800px" 
        InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
        WaitMessageFont-Size="14pt" Width="700px" SizeToReportContent="True">
        <LocalReport ReportPath="BenchmarkStatsRep4c.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetBenchmarkStats" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
    SelectMethod="GetData" 
    TypeName="Benchmark Instant Reports 2.DataSetTableAdapters.TEMP_RESULTS_BENCHMARKSTATSTableAdapter">
</asp:ObjectDataSource>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
</asp:Content>
