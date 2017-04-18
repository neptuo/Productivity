<%@ Page Language="C#" CodeFileBaseClass="System.Web.UI.Page" %>
<!doctype html>
<html>
    <head>
        <title>Web Form</title>
        <link href="<%= ResolveUrl("StyleSheet1.scss") %>" rel="stylesheet" type="text/css" />
    </head>
    <body>a
        <asp:LinkButton PostBackUrl="~/CSharp/Class1.cs" Text="Go to Class1.cs" runat="server" />

        <a href="../CSharp/Class1.cs">He</a>
    </body>
</html>