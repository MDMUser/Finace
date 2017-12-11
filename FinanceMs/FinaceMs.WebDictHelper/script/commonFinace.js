var commonFince = {
    /**
    * 生成查询的参数对象
    * @param obj 查询属性对象
    * @returns {*}
    */
    setParaObj: function (obj) {
        var objPara = new Object();
        objPara.Lbracket = "";
        objPara.Compare = obj.Compare;
        objPara.Field = obj.Field;
        objPara.DataType = obj.DataType;
        objPara.Value = obj.Value;
        objPara.DisplayValue = obj.DisplayValue;
        objPara.Rbracket = "";
        objPara.Relation = "and ";
        objPara.IsCanChange = true;
        objPara.ConvertUpperToCompare = false;
        objPara.Expresstype = 0;
        objPara.FieldCaption = "";
        objPara.Owner = "";
        objPara.Description = "";
        return objPara;
    },
    /**
    * 针对级次调整功能局部刷新树形数据
    * 传了三个参数（$treeList,self,newParentObj）
    */
    partialRefreshForAdjust: function ($treeList, self, newParentObj) {
        var treegrid = $treeList,
            model = self.defaultModel(),
            idKey = self.treeGridHelper.getIDField(model),
            layerKey = model.layerKey,
            node = $treeList.treegrid('getSelected'),
            layerValue = parseInt(node[layerKey], 10),
            oldParentObj = $treeList.treegrid('getParent', node.FJM);
        // 局部刷新 old parent
        if (node.Layer == 1) { // 当前点的级数的值等于1的时候
            // 移除当前的数据 
            self.treeGridHelper.removeNode(treegrid, model, node);
            self.getListDataSourceWithOtherFilterCondition(newParentObj.FJM, newParentObj.Layer + 1).then(function (ds) {
                if (ds) {
                    self.treeGridHelper.appendNodeList(treegrid, newParentObj, model, ds.tables(0).peek(), false);
                    self.treeGridHelper.dealExpand(treegrid, model, newParentObj.FJM);
                }
            })
        }
        else if (newParentObj == null) { //新父节点为空的时候 
            self.Method_Load();
        }
        else {
            // 局部刷新 old parent  
            self.getListDataSourceWithOtherFilterCondition(oldParentObj.FJM, layerValue).then(function (ds) {
                if (ds) {
                    self.treeGridHelper.appendNodeList(treegrid, oldParentObj, model, ds.tables(0).peek(), false);
                    self.treeGridHelper.dealExpand(treegrid, model, oldParentObj.FJM);
                }
            });
            // 局部刷新 new parent  
            self.getListDataSourceWithOtherFilterCondition(newParentObj.FJM, newParentObj.Layer + 1).then(function (ds) {
                if (ds) {
                    self.treeGridHelper.appendNodeList(treegrid, newParentObj, model, ds.tables(0).peek(), false);
                    self.treeGridHelper.dealExpand(treegrid, model, newParentObj.FJM);
                }
            });
        }
    }
};