function ChangeHLText(HL)
{
	try
	{
		
		var HLObj = document.all(HL);
		if ( HLObj != null )
		{
			if ( HLObj.innerHTML == "<A>+</A>")
			{
				HLObj.innerHTML = "<A>-</A>";
				
			}
			else
			{
				HLObj.innerHTML = "<A>+</A>";
			}
		}
	}
	catch(e)
	{}
}

function SetExpanded(HL, HTB)
{
	try
	{
		var IsExpanded = false;
		var HTBObj = document.all(HTB);
		var HLObj = document.all(HL);
		
		if ( HLObj != null && HTBObj != null)
		{
		
			if ( HLObj.innerHTML == "<A>+</A>")
			{
				IsExpanded = false;	
			}
			else
			{
				IsExpanded = true;
			}
					
			
			var ExpandedData = HTBObj.value;
			if ( ExpandedData == null )
			{
				ExpandedData = "";
			}
			
			//===============================================================
			if ( ExpandedData.length < 1 && IsExpanded == true )
			{
				//No Previous Expanded Data.  
				//Add new Expanded Field.
				ExpandedData = HL;
				HTBObj.value = ExpandedData;
			}
			else if ( ExpandedData.length < 1 && IsExpanded == false )
			{
				//No Previous Expanded Data.  
				//Clean up old Expanded Field. 
				//But ExpandedData is empty, so no work is needed.
			}
			
			else if ( ExpandedData.length > 0 && ExpandedData.indexOf(HL) == -1 && IsExpanded == true )
			{
				//Expanded data has data from before.
				//No existing record exists for this field. 
				//We can add new Expanded field. 
				//We can use a comma is a delimeter.
				ExpandedData = ExpandedData + "," + HL;
				ExpandedData = ExpandedData.replace( ",," , ",");
				HTBObj.value = ExpandedData;
			}
			else if ( ExpandedData.indexOf(HL) > -1 && IsExpanded == true )
			{
				//Expanded data has data from before. 
				//Existing record exists for this field. 
				//We do not need to perform any updates.
			}
			else if ( ExpandedData.indexOf(HL) > -1 && IsExpanded == false )
			{
				//Expanded data has data from before.
				//Existing record exists for this field
				//We remove it as it is not expanded any longer.
				ExpandedData = ExpandedData.replace(HL,"");
				
				//Make sure we don't have a double delimeter
				ExpandedData = ExpandedData.replace( ",," , ",");
				HTBObj.value = ExpandedData;
			}
			else if ( ExpandedData.indexOf(HL) == -1 && IsExpanded == false )
			{
				//Expanded data has data from before.
				//Existing record does not exists for this field
				//No work is needed.
			}
			//===============================================================
		}	
	}
	catch(e)
	{ }
}
function HidePanel(Panel)
{
	try
	{
		var ChosenPanel = document.all(Panel);
		if ( ChosenPanel != null )
		{
			document.all(Panel).style.display = "none";
		}
	}
	catch(e)
	{}
}
	
function ShowPanel(Panel)
{
	try
	{
		var ChosenPanel = document.all(Panel);
		if ( ChosenPanel != null )
		{
			document.all(Panel).style.display = "block";
		}
	}
	catch(e)
	{}
}

function HideShowPanel(Panel)
{
	try
	{
		var ChosenPanel = document.all(Panel);
		if ( ChosenPanel != null )
		{
			var currentdisplay = document.all(Panel).style.display;
			if ( currentdisplay != "block")
			{
				document.all(Panel).style.display = "block";
			}
			else
			{
				document.all(Panel).style.display = "none";
			}
		}
	}
	catch(e)
	{}
}

function ModifyStateOfVisibilityOnServer(TextBoxID,Panel)
{
	try
	{
		var TB = document.all(TextBoxID);
		var PNL = document.all(Panel);
		if ( TB != null && PNL != null )
		{
			var currentdisplay = document.all(Panel).style.display;
			if ( currentdisplay == "block")
			{
				TB.value = "block";
			}
			else
			{
				TB.value = "none";
			}
		}
	}
	catch(e)
	{}
}

function bindSubproductosList() {
            var filter = document.form1.txtFiltroProducto.value;
            if (filter.length >= 3) {
                Anthem_InvokePageMethod('bindSubproductos', [filter]);
                document.form1.txtFiltroProducto.focus();
            } else {
                Anthem_InvokePageMethod('limpiarItems', [filter]);
                document.form1.ddlSubproducto.options.length = 1;
                document.form1.ddlSubproducto.options[0] = new Option("Seleccione un Producto", "0");
                document.getElementById("lblNumProductos").innerHTML = "0 Producto(s) Cargado(s)";
            }
}  


function validarEliminacion(){
	      if (confirm("Estas Seguro de Eliminar este registro?")){
		     return(true)
	      }
	      else{
	         return(false)
	      }
}