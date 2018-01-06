//File dialog stuff
//Will class it out later

$(function () {
	//this area is for the file dialog
	//active jquery bindings

	//opens the file browser
	$(".package_open").click(function(){
		if(ncdb.file_browser.loaded==false) {
			$("#app_open_package").load("file_dialog.html",function(){
				ncdb.file_browser.loaded=true;
				    $("#file-browser-show-hidden-folders").bootstrapToggle();
				    $("#file-browser-show-hidden-files").bootstrapToggle();
				    $("#file-browser-ncdb-only").bootstrapToggle();
				});
		}

		$("#server_running").addClass("hidden");
		$(".app_page").hide();
		$("#app_open_package").show();
		ncdb.file_browser.active_file=null;
		file_dialog_load(null);


	});

	//things that case the browser to be redrawn
	$("#app_open_package").on("change","#file-browser-show-hidden-folders" ,function() {  file_dialog_refresh(); });
	$("#app_open_package").on("change","#file-browser-show-hidden-files"   ,function() {  file_dialog_refresh(); });
	$("#app_open_package").on("change","#file-browser-ncdb-only"           ,function() {  file_dialog_refresh(); });


	//things that cause actions to be preformed
	$("#app_open_package").on("click","#folder-tree > .folder-row",function(){
			var folder=$(this).data("folder");
			file_dialog_load(folder);
		});
	$("#app_open_package").on("click","#file_browser > .file",function(){
		var file=$(this).data("file");
		ncdb.file_browser.active_file	=file;
		$("#file_browser tr").removeClass("file-active");
		$(this).addClass("file-active");
	});
	$("#app_open_package").on("click","#file-dialog-ok",function(){
		if(null===ncdb.file_browser.active_file) {
			info("File Dialog","Gotta pick a file first captain.");
			return;
		}
		alert(ncdb.file_browser.active_file);
	});
	$("#app_open_package").on("click","#file-dialog-cancel",function(){
		go_to_last();
	});




   //Support functions
   var get_file_item_icon=function(name,type){
			var special_dirs=[
					{ name:"nocodedb"     ,icon:"fas fa-database"      ,type:"folder" ,ext:"",special:true},
					{ name:"Desktop"      ,icon:"fas fa-desktop"      ,type:"folder" ,ext:"",special:true},
					{ name:"Downloads"    ,icon:"fas fa-download"     ,type:"folder" ,ext:"",special:true},
					{ name:"Documents"    ,icon:"fas fa-file"         ,type:"folder" ,ext:"",special:true},
					{ name:"My Documents" ,icon:"fas fa-file"         ,type:"folder" ,ext:"",special:true},
					{ name:"Pictures"     ,icon:"fas fa-camera"       ,type:"folder" ,ext:"",special:true},
					{ name:"My Pictures"  ,icon:"fas fa-camera"       ,type:"folder" ,ext:"",special:true},
					{ name:"Home"	      ,icon:"fas fa-home"         ,type:"folder" ,ext:"",special:true},
					{ name:"Projects"     ,icon:"fas fa-code-branch"  ,type:"folder" ,ext:"",special:true},
					{ name:""		      ,icon:"fas fa-folder"       ,type:"folder" ,ext:"",special:false},
					{ name:""		      ,icon:"fas fa-file"         ,type:"file"   ,ext:"",special:false}
			];
		var default_unk ={ name:""		      ,icon:"fas fa-file"         ,type:"file"   ,ext:"",special:false}
		//to lower works for windows, for linux its wrong... but right now we will let it slide..
		for(i in special_dirs) {
			if(special_dirs[i].type===type &&
			   special_dirs[i].name.toLowerCase()===name.toLowerCase()) {
				return special_dirs[i];
			}
		}
		for(i in special_dirs) {
			if(special_dirs[i].type==type &&
			   special_dirs[i].name=="") {
				return special_dirs[i];
			}
		}
		return default_unk;		//this is the default icon...?
	}

	var file_dialog_refresh=function(){
		draw_browser();
	}

	var file_dialog_load=function(path) {
		if(path!== null & path!== undefined) {
			if(ncdb.file_browser.files.path==="/") {
				path=ncdb.file_browser.files.path+path;
			} else {
				path=ncdb.file_browser.files.path+"/"+path;
			}
		}

		ncdb.load("api/get_dir",function(jr) {
			ncdb.file_browser.files=jr;
			draw_browser();
		}
		,function(jr){ info("File Error","Bummer... Error loading the file browser data."); },{path:path});
	}//end function


	var draw_browser=function(){
			var hidden_folders    =$("#file-browser-show-hidden-folders").is(':checked');
			var hidden_files      =$("#file-browser-show-hidden-files")  .is(':checked');
			var hide_other_files  =$("#file-browser-ncdb-only")          .is(':checked');

			var d=ncdb.file_browser.files.directories;
			var f=ncdb.file_browser.files.files;

			$("#file_browser").children().remove();							//clear all sources
			$("#folder-tree") .children().remove();							//clear all sources


			if(d===undefined || d===null || d.length===0) {					//no previously saved data sources.
			}

			$("#folder-path").text(ncdb.file_browser.files.path);			//set the header path
			var text="";
			var folder="";

			if(f===null || f===undefined || f.length===0) {					//no files? bummer
				$("#file_browser").append("<tr><td colspan=4>No Files</td></tr>");
			}

			if(ncdb.file_browser.files.path!=null &&
			   ncdb.file_browser.files.path!="/") {							//do we have a up dir selection?
				$("#folder-tree").append('<div class="row folder-row"  data-folder=".."><div class="col"><i class="mr-2 fas fa-level-up-alt fa-1x" aria-hidden="true"></i><div class="file-dialog-text">..</div></div>');

			}

			//build folder and file list
			var file_item ={};
			var e;
			var special	  =false;
			var ei=null;
			//the gammit of possiblities.. clean up later...
			if(null==d && null==f) e=[];
			if(null==d && null!=f) e=f;
			if(null==f && null!=d) e=d;
			if(null!=f && null!=d) e=d.concat(f);

			for(l=0;l<3;l++){
				if(special===true) {
					$("#folder-tree").append('<div class="row bg-light border border-left-0 border-right-0 border-top-0 border-secondary mb-2 mt-2"></div>');
					special=false;
				}

				for(i in e){
					ei=e[i];
					file_item=get_file_item_icon(ei.name,ei.type);

					name=ei.name; 				name =name .replace(" ","&nbsp;");
					if(l==2 && file_item.type==="file") {
						mod =ei.modified;			mod  =mod  .replace(" ","&nbsp;");
						size=ei.size; 				size =size .replace(" ","&nbsp;");
						file=ei.path;
					}

					switch(l) {
						case 0: 	if(l==0 && file_item.type==="folder" &&  file_item.special) {
										if(ei.hidden && !hidden_folders) continue;			//dont show hidden folders
										special=true;
										$("#folder-tree").append(
										'<div class="row folder-row"  data-folder="'+ei.name+'">'+
										'<div class="col"><div class="my-auto folder-special"><i class="mr-2 '+file_item.icon+' fa-1x" aria-hidden="true"></i></div><div class="file-dialog-text">'+name+'</div></div>'+
										'');
									} break;
						case 1: 	if(l==1 && file_item.type==="folder" &&  !file_item.special) {
										if(ei.hidden && !hidden_folders) continue;			//dont show hidden folders
										$("#folder-tree").append(
										'<div class="row folder-row"  data-folder="'+ei.name+'">'+
										'<div class="col"><div class="my-auto folder"><i class="mr-2 '+file_item.icon+' fa-1x" aria-hidden="true"></i></div><div class="file-dialog-text">'+name+'</div></div>'+
										'');
									} break;
						case 2: 	if(l==2 && file_item.type==="file") { //no specials for files yet
										if(ei.hidden && !hidden_files) continue;
										if(hide_other_files && ei.extension!=ncdb.file_browser.default_extension) continue;			//dont show hidden folders

										$("#file_browser").append(
											'<tr scope="row" class="file" data-file="'+ei.name+'">'+
											'<td class="clear col-sm-8 "><i class="mr-2 '+file_item.icon+' fa-1x" aria-hidden="true"></i>'+name+'</td>'+
											'<td class="col-2 text-right">'+size+'</td>'+
											'<td class="col-2 text-right">'+mod+'</td>'+
										'</tr>');
									}
									break;
					} //end switch
				}//end for in d
			}//end outside loop
		}//end ncdb function (ajax load)

	ncdb.file_browser={};
	ncdb.file_browser.files		 		 ={};
	ncdb.file_browser.files.path 		 ={};
	ncdb.file_browser.files.directories  ={};
	ncdb.file_browser.files.files		 ={};
	ncdb.file_browser.default_extension  =".ncdb.json";
	ncdb.file_browser.loaded	 		 =false;
	ncdb.file_browser.active_file		 =null;

});
