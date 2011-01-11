<%@ Page Title="BIR: Student Summary Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentSummary.aspx.cs" Inherits="Benchmark_Instant_Reports_2.WebForm4" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2
        {
            width: 240px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        The Student Summary Report shows student test scores and a question-by-question 
        view of their test answers. This report is designed to be cut into small 
        sections and given to each student.</p>
    <p>
    <table style="width: 95%;">
        <tr>
            <td class="style13" valign="middle">
                        <table style="width:97%;">
        <tr>
            <td class="style13" valign="middle" align="right">
                Select Campus:</td>
            <td align="left" class="style2" valign="middle">
    <asp:DropDownList ID="ddCampus" runat="server" Height="28px" Width="240px" 
    onselectedindexchanged="ddCampus_SelectedIndexChanged1">
    </asp:DropDownList>
            </td>
            <td class="style12">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style13" valign="middle" align="right">
    <asp:Label ID="lblEnterSchoolPassword" runat="server" Font-Bold="True" 
        ForeColor="#3366FF" Text="Enter school password:"></asp:Label>
            </td>
            <td align="left" class="style2" valign="middle">
                <asp:TextBox ID="txtbxSchoolPassword" runat="server" Width="68px" 
                    TextMode="Password"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnEnterPassword" runat="server" 
        onclick="btnEnterPassword_Click" Text="Submit" />
            </td>
            <td class="style12">
    <asp:Label ID="lblIncorrectPassword" runat="server" Font-Bold="False" 
        ForeColor="#FF3300" Text="Incorrect school password."></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style13" valign="middle" align="right">
                Select Benchmark:</td>
            <td align="left" class="style2" valign="middle">
    <asp:DropDownList ID="ddBenchmark" runat="server" Height="28px" 
    Width="240px" onselectedindexchanged="ddBenchmark_SelectedIndexChanged1">
    </asp:DropDownList>
            </td>
            <td class="style12">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style13" valign="middle" align="right">
                &nbsp;</td>
            <td align="left" class="style2" valign="middle">
                &nbsp;</td>
            <td class="style12">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style13" valign="middle" align="right">
                Select Teacher:</td>
            <td align="left" class="style2" valign="middle">
    <asp:DropDownList ID="ddTeacher" runat="server" Height="28px" Width="240px" 
    onselectedindexchanged="ddTeacher_SelectedIndexChanged1">
    </asp:DropDownList>
            </td>
            <td class="style12">
                &nbsp;</td>
        </tr>
    </table>
  </td>
            <td valign="middle" class="style18">
                    <table style="width: 82%; margin-left: 6px;">
        <tr>
            <td class="style16">
                Select Period(s):</td>
            <td class="style15">
                <asp:ListBox ID="lbPeriod" runat="server" Rows="7" 
                    onselectedindexchanged="lbPeriod_SelectedIndexChanged1" Width="50px"></asp:ListBox>
            </td>
        </tr>
        </table></td>
        </tr>
        <tr>
            <td class="style13" valign="middle">
                        &nbsp;</td>
    </strong>
            <td valign="middle" class="style17">
                    Use SHIFT and CTRL keys for multiple values</td>
        </tr>
    <strong __designer:mapid="189">
        <tr>
            <td class="style13" align="center" valign="middle">
                        <asp:Button ID="btnGenReport" runat="server" Text="Generate Report" 
    onclick="btnGenReport_Click" />
  </td>
            <td valign="middle" class="style18">
                    &nbsp;</td>
        </tr>
        <tr>
            <td class="style13" align="center" valign="middle">
    <asp:Label ID="lblAlignmentNote" runat="server" Font-Bold="False" 
        ForeColor="#FF3300" 
                            Text="** Report is optimally formatted for printing from Internet Explorer and may not appear to be aligned correctly on screen"></asp:Label>
  </td>
            <td valign="middle" class="style18">
                    &nbsp;</td>
        </tr>
        </table>










    </p>
    <p>
        </p>
    <rsweb:ReportViewer ID="repvwStudentSummary" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
        SizeToReportContent="True" WaitMessageFont-Names="Verdana" 
        WaitMessageFont-Size="14pt">
        <localreport reportpath="StudentSummaryRep.rdlc">
            <datasources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSetStudentStats" />
            </datasources>
        </localreport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        DeleteMethod="Delete" InsertMethod="Insert" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByUseFilter" 
        TypeName="Benchmark_Instant_Reports_2.DataSetStudentStatsTableAdapters.TEMP_RESULTS_STUDENTSTATSTableAdapter" 
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

</asp:Content>
