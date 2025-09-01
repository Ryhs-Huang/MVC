function preview(inputType) {
    var file = inputType.files[0]; // 取得上傳的第一個檔案
    var allowTypes = "image.*";    // 允許的檔案類型 => 所有的圖片檔
    if (file.type.match(allowTypes)) {
        $(".btn").prop("disabled", false);   // 讓 submit 鍵可以按
        var reader = new FileReader();      // FileReader 可以讀 User 上傳的檔案
        reader.onload = function (e) {        // 當讀取完成後
            $("#Picture").prev().attr("src", e.target.result); // 把讀到的檔案放到 img 的 src
            $("#Picture").prev().attr("title", file.name);      // 把檔名放到 img 的 title

        };
        reader.readAsDataURL(file);        // 以 DataURL 的方式讀取檔案
    }
    else {
        alert("不允許的檔案上傳類型");         // 只有顯示警示，選完上傳仍會顯示檔名
        $(".btn").prop("disabled", true);   // 讓 submit 鍵無法按
        inputType.value = "";               // 清空 input file 的值
        $("#Picture").prev().attr("src", "/images/noimage.jpg");  // 只有 Razor 可以用 ~，JS 要用 /
        $("#Picture").prev().attr("title", "尚無圖片");            // 清空預覽圖片
    }
    // 如果上傳的副檔名正確，抓預覽的圖，有設 id 可以用 id 抓，沒有可以用相對位置抓
    // 如果上傳的副檔名不對，不給點 submit 鍵
}
$("#Picture").on("change", function () {
    // alert("change");
    preview(this);
    // 這裡的this是指input file(檔案上傳元素)
});