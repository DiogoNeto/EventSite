
<script type="text/javascript">
$(window).scroll(function()
{
    if($(window).scrollTop() == $(document).height() - $(window).height())
    {
        $('div#loadmoreajaxloader').show();
        $.ajax({
            url: "loadmore.php",
            success: function(html)
            {
                if(html)
                {
                    $("#postswrapper").append(html);
                    $('div#loadmoreajaxloader').hide();
                }else
                {
                    $('div#loadmoreajaxloader').html('<center>No more posts to show.</center>');
                }
            }
        });
    }
});
</script>