/*$(function () {
           $('textarea').redactor({
               minHeight: 400,
               wym: true
           });
       });
*/

$(function () {
    $("textarea.mdd_editor").MarkdownDeep({
        help_location: '/Scripts/mdd_help.htm',
        ExtraMode: true,
        height: '400px'
    });

    $('.mdd_preview').css({ height: $(window).height() });
    

    $('#submitBtn').click(function () {
        $('#updateForm').submit();
    });
});