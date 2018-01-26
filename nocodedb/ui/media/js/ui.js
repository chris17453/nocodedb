
	//	load_sources_success("bob");
$(function () {
	var package_settings_fragment="./media/html/package_settings.html";
	//this area is for the NEW PACKAGE page
	$(".package_settings").click(function(){
		 open_package_settings();
	});
	$(".package_new").click(function(){
		ncdb.package={};
		$("#package_name").val("");														//reset data sources form
		$("#package_data_source_name").val("");											
		$("#package_data_source_host").val("");
		$("#package_data_source_user").val("");
		$("#package_data_source_password").val("");
		$("#package_data_source_type").val("");
		$("#pacakage_data_sources").children("option:not(:first)").remove();			//clear all sources
		ncdb.sources={};
		

		 open_package_settings();
	});



	var update_data_sources=function(jr){
			var d=jr.sources
			$("#pacakage_data_sources").children("option:not(:first)").remove();			//clear all sources
			ncdb.sources={};
		
			if(d===undefined || d===null || d.length===0) {				//no previously saved data sources.
//					alert("No data");
//					return;
			}
			ncdb.sources=d;
			for(i in d){
				
				$("#pacakage_data_sources").append($('<option>', { 
					value:d[i].name,
					text :d[i].name 
				}));
			}
		};

	
	//This area is for the PACKAGE_SETTINGS page.
	var open_package_settings=function(){
		$("#server_running").addClass("hidden"); 
		$(".app_page").hide();
		$("#app_package_settings").removeClass("hidden");
		$("#app_package_settings").show();
		if(!ncdb.package_settings.loaded) {
			$("#app_package_settings").load(package_settings_fragment,function(){
					ncdb.package_settings.loaded=true;
			});
		}
		ncdb.load("api/get_data_sources",update_data_sources);
	}  
	
	$("#app_package_settings").on("click" ,"#data_sources_remove",function(){
		var selectedValue = $("#pacakage_data_sources").val();
		if(selectedValue===null || selectedValue==undefined || selectedValue=="") return;
		var obj={};
		for(i in ncdb.sources){
			if(ncdb.sources[i].name==selectedValue) {
				obj=ncdb.sources[i];
			}
		}
		
		ncdb.load("api/remove_data_source",update_data_sources,function(jr){  err("Error removing data source"); },obj);
		
	});
	$("#app_package_settings").on("change","#pacakage_data_sources",function(){
		var selectedText = $(this).find("option:selected").text();
		var selectedValue = $(this).val();
	  
		var source=selectedValue;
		if(source===undefined || source==null || source=="") {
			$("#package_data_source_name").val("");
			$("#package_data_source_host").val("");
			$("#package_data_source_user").val("");
			$("#package_data_source_password").val("");
			$("#package_data_source_type").val("");
		} else {
			var obj=null;
			for(i in ncdb.sources){
				if(ncdb.sources[i].name==source) {
					obj=ncdb.sources[i];
				}
			}
			if(obj!==null) {
				$("#package_data_source_name").val(obj.name);
				$("#package_data_source_host").val(obj.host);
				$("#package_data_source_user").val(obj.user);
				$("#package_data_source_password").val(obj.password);
				$("#package_data_source_type").val(obj.type);
			}
		}
	});
	$("#app_package_settings").on("click" ,"#package_data_source_save",function(){
		var package_name=$("#package_name").val();
		var name=$("#package_data_source_name").val();
		var host=$("#package_data_source_host").val();
		var user=$("#package_data_source_user").val();
		var pass=$("#package_data_source_password").val();
		var type=$("#package_data_source_type").find(":selected").val();
		var errors=[];
		if($.isBlank(name)) { errors.push('The type must be set');  }
		if($.isBlank(package_name)) { errors.push('The package name must be set');  }
		if($.isBlank(name)) { errors.push('The data source name must be set');  }
		if($.isBlank(host)) { errors.push('The host must be set'); }
		if($.isBlank(user)) { errors.push('The user must be set'); }
		if($.isBlank(pass)) { errors.push('The password must be set'); }
		if(errors.length>0) {
			  var title = "Data Source Error";
			  var body = errors.join("<br>");
			  info(title,body);
			return;
		}
		var source={};
		source.name=name;
		source.host=host;
		source.user=user;
		source.password=pass;
		source.type=type;
		ncdb.package.src=source;
		ncdb.package.name=package_name;
		ncdb.load("api/add_data_source",function(jr){ 
			//var ncdb=this.ncdb;
				if(!jr) {
				  var title = "Data Source Error";
				  var body = "Connection Failed";
				  info(title,body);
				} else {
				  var title = "Data Source";
				  var body = "Connected. Good job.";
				  ncdb.load("api/new_package",function(jr){ 
						  if(!jr) {
							info("Package Error","WTH! Save failed! Check things like permissions and drive space.");
						  } else {
							info("Package","Package saved and data source verified.");  
						  }
					 },
				  function(jr){ info("Package Error","API Failed! Did yo turn something off?"); },ncdb.package);
		
				  
				}
			},
			function(jr){ info("Data Source","Error saving recent data sources"); },
			source);
	});

});
