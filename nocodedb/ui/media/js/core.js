var ncdb=(function(my){
		my.webServiceURL    ="http://localhost:52000";
		my.data_object      =my.data_object||{};                 
		my.package			={};
		my.load =function(method,success_func,err_func,da){
			if(da!==undefined && da!==null)  {
				my.data_object=da;
			} else my.data_object={};
			$.ajax({type: "POST", url: my.webServiceURL +"/" + method, data:JSON.stringify(my.data_object),
					success: function(results){	if(success_func){success_func(results);}}, 
					error: function(results){	if(err_func){ err_func(results); } else err(results); }, 
					contentType: "application/json; charset=utf-8",
					dataType: "json" });
		};
		my.obj_to_string=function(obj){
			return  JSON.stringify(obj);                                                    //needs everything to be an object... {} not []
		};
		my.get_heartbeat=function(){
			my.load("api/heartbeat",function(jr){ 
				if(jr!=="") {
					 $("#server_running").removeClass("hidden"); 
					 $("#navbar_menu")   .removeClass("hidden"); 
					 $("#navbar_buttons").removeClass("hidden"); 
					 
					 my.WebPI=true; 
				} else { 
					// $("#server_not_running").removeClass("hidden"); 
					 my.WebPI=false;
				} 
			},function(jr){ 
				my.WebPI=false;
				//$("#server_not_running").removeClass("hidden"); 
				});
		}
		return my;
	}(ncdb || {}));//auto execute the function, closure.. etc..

	var err=function(jr){
		info("Oops","Something broke...");
	}

	var info=function(title,body){
		$("#myModal").modal("show");
		$('#info_model_title').html(title);
		$('#info_model_body').html(body);
	}

	var go_to_last=function(){
		$(".app_page").hide();
		$("#app_home").show();	
	}	//package defaults



$(function(){
		$("body").on("click",".app_home",function(){ 
		$(".app_page").hide();
		$("#app_home").show();
	});
	$(".navbar-brand").on("click",function(){ 
		$(".app_page").hide();
		$("#app_home").show();
	});
	$.isBlank = function(obj){
		return(!obj || $.trim(obj) === "");
	};


	ncdb.package_settings={};
	ncdb.package_settings.loaded=false;
	//Activate the interactive system (show the menu bar)
	ncdb.get_heartbeat();		//is the service active?

});
