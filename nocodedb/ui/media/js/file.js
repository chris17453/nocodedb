

$(function () {

	$(".package_open").click(function(){
		$("#server_running").addClass("hidden");
		$(".app_page").hide();
		$("#app_open_package").show();
		$(".server_file_dialog").browsy({path:null,filters:[".png"],admin_filters:true,server:"http://localhost:8888"});
	});
	


});//end on document ready
