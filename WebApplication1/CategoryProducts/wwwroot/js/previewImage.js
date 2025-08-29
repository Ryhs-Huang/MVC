function preview(inputType) {
    var file = inputType.files[0];
    var allowTypes = "image.*";//限制附檔名為image類型的
    if (file.type.match(allowTypes)) {
        $(".btn").prop("disabled", false);
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#Picture").prev().attr("src", e.target.result);
            $("#Picture").prev().attr("title", file.name);
        };
        reader.readAsDataURL(file);
    }
    else {
        alert("不允許的檔案上傳類型");
        $(".btn").prop("disabled", true);
        inputType.value = "";
        $("#Picture").prev().attr("src", "/images/noimage.jpeg");
        $("#Picture").prev().attr("title", "尚無圖片");
    }
}
$("#Picture").on("change", function () {
    //alert("change");
    preview(this);
});