//项目自定义js
//import "./libs/xxxx.js";

//导出打印预览表格为Excel(全局函数,Known框架自动加载)
window.exportBillToExcel = function () {
    const el = document.querySelector('.bill-print');
    if (!el) {
        alert('未找到打印预览内容');
        return;
    }
    const clone = el.cloneNode(true);
    //输入框值转为文本
    clone.querySelectorAll('input').forEach(input => {
        const span = document.createElement('span');
        span.textContent = input.value || '';
        input.replaceWith(span);
    });
    //去掉编辑辅助元素
    clone.querySelectorAll('.width-bar, .merge-tip, button').forEach(e => e.remove());
    const html = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel"><head><meta charset="UTF-8"></head><body>' + clone.outerHTML + '</body></html>';
    const blob = new Blob(['\ufeff' + html], { type: 'application/vnd.ms-excel;charset=utf-8' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = '送货单.xls';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
};

export class AppRazor {
    static test(message) {
        alert(message);
    }
}
