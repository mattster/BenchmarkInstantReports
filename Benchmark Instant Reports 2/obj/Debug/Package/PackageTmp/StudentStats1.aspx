<%@ Page Title="BIR: Student Stats Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentStats1.aspx.cs" Inherits="Benchmark_Instant_Reports_2.WebForm1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #pwbxSchoolPassword
        {
            width: 63px;
        }
        .style2
        {
            width: 201px;
        }
        .style12
        {
            width: 619px;
        }
        .style13
        {
            width: 608px;
        }
        .style15
        {
            width: 311px;
        }
        .style16
        {
            width: 147px;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <strong __designer:mapid="189">
    The Student Statistics Report shows individual student grades for a 
specific test, teacher and period.<br />
    <br />
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
        </table>
    </strong>
<br />
<br />
<rsweb:ReportViewer ID="repvwStudentStats1" runat="server" Font-Names="Verdana" 
    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="800px" 
        Height="600px" SizeToReportContent="True">
    <LocalReport ReportPath="StudentStatsRep2.rdlc">
        
        <DataSources>
            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                Name="DataSetStudentStatsRep2" />
        </DataSources>
        
    </LocalReport>
</rsweb:ReportViewer>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
    SelectMethod="GetData" 
    
        TypeName="Benchmark_Instant_Reports_2.DataSetStudentStatsTableAdapters.TEMP_RESULTS_STUDENTSTATSTableAdapter" 
        DeleteMethod="Delete" InsertMethod="Insert" 
        OldValuesParameterFormatString="original_{0}" UpdateMethod="Update">
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
