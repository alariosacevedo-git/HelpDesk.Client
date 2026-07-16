var _DashBoard = {
    onMainMenuActive: function (LiParent, LiChild) {
        var liParent = LiParent;
        var liChild = LiChild;

        if (liParent != "Null") {
            $("#"+ LiParent +"").addClass("active open");
        }

        if (LiChild != "Null") {
            $("#" + liChild + "").addClass("active");
        }
    }
};
