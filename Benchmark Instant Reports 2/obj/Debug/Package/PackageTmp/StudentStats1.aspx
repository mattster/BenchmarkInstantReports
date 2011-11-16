<%@ Page Title="BIR: Student Grades" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="StudentStats1.aspx.cs" Inherits="Benchmark_Instant_Reports_2.WebForm1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #pwbxSchoolPassword
        {
            width: 63px;
        }
        .style12
        {
            width: 619px;
        }
        .style13
        {
            width: 667px;
        }
        .style17
        {
            font-size: small;
            text-align: left;
            width: 292px;
        }
        .style18
        {
            width: 292px;
        }
        .style19
        {
            width: 470px;
        }
        .style20
        {
            width: 582px;
        }
        .style21
        {
        }
        .style22
        {
            width: 179px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Student Grades</h1>
    <p>
        The Student Statistics Report shows individual student grades for a specific test,
        teacher and period.<br />
    </p>
    <br />
    <table style="width: 95%;">
        <tr>
            <td class="style13" valign="middle">
                <table style="width: 100%;">
                    <tr>
                        <td class="style19" valign="middle" align="right">
                            Select Campus:
                        </td>
                        <td align="left" class="style20" valign="middle">
                            <asp:DropDownList ID="ddCampus" runat="server" Height="28px" Width="240px" OnSelectedIndexChanged="ddCampus_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style19" valign="middle" align="right">
                        </td>
                        <td align="left" class="style20" valign="middle">
                        </td>
                    </tr>
                    <tr>
                        <td class="style19" valign="middle" align="right">
                            Select Test:
                        </td>
                        <td align="left" class="style20" valign="middle">
                            <asp:DropDownList ID="ddBenchmark" runat="server" Height="28px" Width="445px" OnSelectedIndexChanged="ddBenchmark_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style19" valign="middle" align="right">
                            &nbsp;
                        </td>
                        <td align="left" class="style20" valign="middle">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style19" valign="middle" align="right">
                            Select Teacher:
                        </td>
                        <td align="left" class="style20" valign="middle">
                            <asp:DropDownList ID="ddTeacher" runat="server" Height="28px" Width="240px" OnSelectedIndexChanged="ddTeacher_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="middle" class="style18">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style13" valign="middle">
                &nbsp;
                <asp:Label ID="lblNoScanData" runat="server" Text="There is no scanned data currently available for this test at this campus"
                    ForeColor="#0000FF"></asp:Label>
            </td>
            <td valign="middle" class="style17">
            </td>
        </tr>
        <tr>
            <td class="style13" align="center" valign="middle">
                <asp:Button ID="btnGenReport" runat="server" Text="Generate Report" OnClick="btnGenReport_Click" />
            </td>
            <td valign="middle" class="style18">
                &nbsp;
            </td>
        </tr>
    </table>
    <br />
    <br />
    <rsweb:ReportViewer ID="repvwStudentStats2a" runat="server" Font-Names="Verdana"
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
        WaitMessageFont-Size="14pt" Width="800px" Height="113px" SizeToReportContent="True">
        <LocalReport ReportPath="StudentStatsRep2a.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetStudentStatsRep2" />
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
