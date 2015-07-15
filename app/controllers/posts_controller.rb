class PostsController < ApplicationController
  before_action :set_post, only: [:show, :edit, :update, :destroy]

  # GET /posts
  # GET /posts.json
  def index
    @posts = Post.all
  end

  # GET /posts/1
  # GET /posts/1.json
  def show
  end

  # GET /posts/new
  def new
    @post = Post.new
  end

  # GET /posts/1/edit
  def edit
  end

  # POST /posts
  # POST /posts.json
  def create
    @post = Post.new(post_params)

    respond_to do |format|
      if @post.save
        format.html { redirect_to @post, notice: 'Post was successfully created.' }
        format.json { render action: 'show', status: :created, location: @post }
      else
        format.html { render action: 'new' }
        format.json { render json: @post.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /posts/1
  # PATCH/PUT /posts/1.json
  def update
    respond_to do |format|
      if @post.update(post_params)
        format.html { redirect_to @post, notice: 'Post was successfully updated.' }
        format.json { head :no_content }
      else
        format.html { render action: 'edit' }
        format.json { render json: @post.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /posts/1
  # DELETE /posts/1.json
  def destroy
    @post.destroy
    respond_to do |format|
      format.html { redirect_to posts_url }
      format.json { head :no_content }
    end
  end

  def events
    events_json = {
      calendar: {
        slot_duration: '00:45:00'
      },
      events: [
        {
          start:  DateTime.new(2015, 07, 17, 10, 30, 00),
          end: DateTime.new(2015, 07, 17, 15, 30, 00) + 1.hours,
          event_type: 'blocked',
          event_details: {
            blocked_for: 'OPD',
            subject: 'foo'
          }
        },
        {
          start:  DateTime.new(2015, 07, 18, 12, 30, 00),
          end: DateTime.new(2015, 07, 18, 14, 30, 00) + 1.hours,
          event_type: 'booking',
          event_details: {
            first_name: 'Poorva',
            last_name: 'Mahajan',
            subject: 'boo'
          }
        },
        {
          start: DateTime.now + 3.hours,
          end: DateTime.now + 5.hours,
          event_type: 'non-working',
          event_details: {}
        }
      ]
    }
    render json: events_json
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_post
      @post = Post.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def post_params
      params.require(:post).permit(:title, :blurb, :body)
    end
end
