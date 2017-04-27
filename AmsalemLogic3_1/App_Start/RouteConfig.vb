Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Public Module RouteConfig
    Public Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        routes.MapRoute(
            name:="Default",
            url:="{controller}/{action}/{id}",
            defaults:=New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional}
        )

        routes.MapRoute(
         name:="Default1",
         url:="{controller}/{action}/{Id}/{Id2}",
         defaults:=New With {.controller = "Home", .action = "Index", .Id = UrlParameter.Optional, .Id2 = UrlParameter.Optional}
     )
    End Sub
End Module