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
        objPara.Relation = obj.Relation;
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
    * @param obj 树表对象
    * @param obj 页面本身
    * @param obj 新的父级对象
    */
    partialRefreshForAdjust: function ($treeList, self, newParentObj) {
        var model = self.defaultModel(),
            idKey = self.treeGridHelper.getIDField(model),
            layerKey = model.layerKey,
            node = $treeList.treegrid('getSelected'),
            layerValue = parseInt(node[layerKey], 10),
            oldParentObj = $treeList.treegrid('getParent', node.FJM);
        // 移除当前的节点
        self.treeGridHelper.removeNode($treeList, model, node);
        // 新父节点为空的时候
        if (newParentObj == null) {
            return self.getListDataSourceWithOtherFilterCondition('', 1)
                  .then(function (ds) {
                      if (ds && ds.tables(0)) {
                          //treegrid加载数据
                          self.treeGridHelper.loadData($treeList, model, ds.tables(0).peek());
                          //选中第一个节点
                          self.selectFirstNode();
                      }
                  })
        }
        else {
            // 局部刷新 new parent  
            self.getListDataSourceWithOtherFilterCondition(newParentObj.FJM, newParentObj.Layer + 1).then(function (ds) {
                if (ds) {
                    if ($treeList.treegrid('find', newParentObj.FJM) != null) {
                        self.treeGridHelper.appendNodeList($treeList, newParentObj, model, ds.tables(0).peek(), false);
                        self.treeGridHelper.dealExpand($treeList, model, newParentObj.FJM);
                    }
                }
            });
        }
    },

    /**
    * 重写平台 TreeCardController.js 中 getListDataSourceWithOtherFilterCondition 方法
    * 根据筛选条件获取数据
    * @param string 平台参数
    * @param int 平台参数
    * @param int 平台参数
    * @param object 自定义参数
    * @param object this
    */
    getListDataSourceWithOtherFilterConditionEx: function (idValue, layerValue, pageSize, ownFilter, self) {
        var filter = self.getFilterWithOtherFilterCondition(idValue, layerValue, pageSize);
        var loadFilter = JSON.parse(filter.filterCondition);
        loadFilter.unshift(ownFilter);
        filter.filterCondition = JSON.stringify(loadFilter);
        $.loading();
        return self.listInstance().load(filter)
            .then(function (ds) {
                return ds;
            })
            .always(function () {
                $.loaded();
            });
    },

    /**
    * 树形控件查询功能
    * @param obj 树表对象
    * @param obj 页面本身
    * @param obj 新的父级对象
    */
    TreeDictSearch: function ($treeList, self, dictName, nmField) {
        var filters = $treeList.treegrid('options').filterRules;
        var model = self.defaultModel(),
            maxCount = 1000,
            listView;
        if (filters != null && filters.length > 0) {
            var paraDialog = new Array();
            paraDialog[0] = dictName;
            paraDialog[1] = JSON.stringify(filters);
            var dataService = self.context.injector.get("$dataServiceProxy");
            // 获取符合条件的内码
            return dataService.invokeMethod("FinaceMs.WebDictHelper.WebSearch", "SearchDataByFilter", paraDialog).then(function (result) {
                var data = result.data;
                if (data != null && data != '') {
                    // 从后台获取全部数据并载入到树表格中
                    var obj = new Object();
                    obj.Compare = " in ";
                    obj.Field = nmField;
                    obj.DataType = "string";
                    obj.Value = "(" + data + ")";
                    obj.DisplayValue = data;
                    obj.Relation = " and ";
                    var objNM = commonFince.setParaObj(obj);
                    return commonFince.getListDataSourceWithOtherFilterConditionEx('', 0, maxCount, objNM, self).then(function (ds) {
                        if (ds && ds.tables(0) && ds.tables(0).rowCount() > 0) {
                            listView = ds.tables(0).defaultView();
                            if (listView.totalRecordsCount() > maxCount) {
                                $.notify.info("数据量过大，请详细查询");
                            }
                            else {
                                self.treeGridHelper.expandAll($treeList, model, ds.tables(0).peek());
                            }
                        }
                    });
                }
            });
        }
        else {
            return self.getListDataSourceWithOtherFilterCondition('', 1)
                    .then(function (ds) {
                        if (ds && ds.tables(0)) {
                            //treegrid加载数据
                            self.treeGridHelper.loadData($treeList, model, ds.tables(0).peek());
                            //选中第一个节点
                            self.selectFirstNode();
                        }
                    })
        }

    }
};