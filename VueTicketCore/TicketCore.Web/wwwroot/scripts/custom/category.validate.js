$("#DepartmentName").blur(function ()
{
    if ("" !== $("#DepartmentName").val())
    {
        $.getJSON("/Administration/Department/CheckDepartmentName",
            { departmentName: $("#DepartmentName").val() },
            function (a) { a && (alert("Department Name already Exists"), $("#DepartmentName").val("")) });
    }
});